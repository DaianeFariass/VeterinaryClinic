using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vereyon.Web;
using VeterinaryClinic.Data;
using VeterinaryClinic.Data.Entities;
using VeterinaryClinic.Enums;
using VeterinaryClinic.Helpers;
using VeterinaryClinic.Migrations;
using VeterinaryClinic.Models;
using VeterinaryClinic.Repositories;

namespace VeterinaryClinic.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentRespository _appointmentRespository;
        private readonly IPetRepository _petRepository;
        private readonly IVetRepository _vetRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IFlashMessage _flashMessage;
        private readonly IConverterHelper _converter;
        private readonly IMailHelper _mailHelper;
        private readonly DataContext _context;

        public AppointmentsController(IAppointmentRespository appointmentRespository,
            IPetRepository petRepository,
            IVetRepository vetRepository,
            INotificationRepository notificationRepository,
            IFlashMessage flashMessage,
            IConverterHelper converter,
            IMailHelper mailHelper,
            DataContext context)
        {
            _appointmentRespository = appointmentRespository;
            _petRepository = petRepository;
            _vetRepository = vetRepository;
            _notificationRepository = notificationRepository;
            _flashMessage = flashMessage;
            _converter = converter;
            _mailHelper = mailHelper;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _appointmentRespository.GetAppointmentAsync(this.User.Identity.Name);
            return View(model);
        }
        [Route("create")]
        public async Task<IActionResult> Create()
        {
            var model = await _appointmentRespository.GetDetailsTempsAsync(this.User.Identity.Name);
            return View(model);
        }
        [Route("addappointment")]
        public IActionResult AddAppointment()
        {

            var model = new AppointmentDetailsViewModel
            {
                Pets = _petRepository.GetComboPets(),
                Vets = _vetRepository.GetComboVets(),
                Date = DateTime.Now.Date,
                Time = DateTime.Now.AddHours(8),
            };
            ViewBag.Pets = model.Pets;
            ViewBag.Vets = model.Vets;
            return View(model);
        }
        [HttpPost]
        [Route("addappointment")]
        public async Task<IActionResult> AddAppointment(AppointmentDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.Date.Date < DateTime.Now.Date)
                {
                    _flashMessage.Warning("Date Invalid!");
                    model = new AppointmentDetailsViewModel
                    {
                        Pets = _petRepository.GetComboPets(),
                        Vets = _vetRepository.GetComboVets(),
                        Date = DateTime.Now.Date,
                        Time = DateTime.Now.AddHours(8),

                    };
                    ViewBag.Pets = model.Pets;
                    ViewBag.Vets = model.Vets;
                    return View(model);
                }
                var appointments = await _appointmentRespository.GetDetailsTempsAsync(this.User.Identity.Name);
                bool appointmentDetailtemp = appointments.Any(a =>
                a.Date == model.Date &&
                a.Time == model.Time &&
                a.Vet.Id == model.VetId);

                if (appointmentDetailtemp)
                {
                    _flashMessage.Warning("The vet in this date and time is unvailable");
                    model = new AppointmentDetailsViewModel
                    {
                        Pets = _petRepository.GetComboPets(),
                        Vets = _vetRepository.GetComboVets(),
                        Date = DateTime.Now.Date,
                        Time = DateTime.Now.AddHours(8),

                    };
                    ViewBag.Pets = model.Pets;
                    ViewBag.Vets = model.Vets;
                    return View(model);

                }
                else
                {
                    await _appointmentRespository.AddItemToAppointmenteAsync(model, this.User.Identity.Name);
                    return RedirectToAction("Create");

                }


            }

            return View(model);
        }
        [Route("editappointment")]
        public async Task<IActionResult> EditAppointment(int? id)
        {
            if (id == null)
            {

                return new NotFoundViewResult("AppointmentNotFound");

            }
            var appointmentToEdit = await _appointmentRespository.GetAppointmentDetailTempAsync(id.Value);

            if (appointmentToEdit == null)
            {
                return new NotFoundViewResult("AppointmentNotFound");

            }
            var model = new AppointmentDetailsViewModel
            {
                Id = appointmentToEdit.Id,
                Pets = _petRepository.GetComboPets(),
                Vets = _vetRepository.GetComboVets(),
                Date = appointmentToEdit.Date,
                Time = DateTime.Now.AddHours(8),
            };
            ViewBag.Pets = model.Pets;
            ViewBag.Vets = model.Vets;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("editappointment")]
        public async Task<IActionResult> EditAppointment(AppointmentDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.Date.Date < DateTime.Now.Date)
                    {
                        _flashMessage.Warning("Date Invalid!");
                        model = new AppointmentDetailsViewModel
                        {
                            Pets = _petRepository.GetComboPets(),
                            Vets = _vetRepository.GetComboVets(),
                            Date = DateTime.Now.Date,
                            Time = DateTime.Now.AddHours(8),

                        };
                        ViewBag.Pets = model.Pets;
                        ViewBag.Vets = model.Vets;
                        return View(model);
                    }
                    var appointmentDetails = await _appointmentRespository.GetDetailsTempsAsync(this.User.Identity.Name);
                    bool appointmentDetailtemp = appointmentDetails.Any(a =>
                    a.Date == model.Date &&
                    a.Time == model.Time &&
                    a.Vet.Id == model.VetId);

                    if (appointmentDetailtemp)
                    {
                        _flashMessage.Warning("The vet in this date and time is unvailable");
                        model = new AppointmentDetailsViewModel
                        {
                            Pets = _petRepository.GetComboPets(),
                            Vets = _vetRepository.GetComboVets(),
                            Date = DateTime.Now.Date,
                            Time = DateTime.Now.AddHours(8),

                        };
                        ViewBag.Pets = model.Pets;
                        ViewBag.Vets = model.Vets;
                        return View(model);

                    }

                    var appointment = await _appointmentRespository.GetAppointmentAsync(this.User.Identity.Name);
                    bool appointmentDetailsTemp = appointment.Any(a =>
                    a.Date == model.Date &&
                    a.Time == model.Time &&
                    a.Vet.Id == model.VetId);

                    if (appointmentDetailsTemp)
                    {
                        _flashMessage.Warning("The vet in this date and time is unvailable");
                        model = new AppointmentDetailsViewModel
                        {
                            Pets = _petRepository.GetComboPets(),
                            Vets = _vetRepository.GetComboVets(),
                            Date = DateTime.Now.Date,
                            Time = DateTime.Now.AddHours(8),

                        };
                        ViewBag.Pets = model.Pets;
                        ViewBag.Vets = model.Vets;
                        return View(model);

                    }

                    await _appointmentRespository.EditAppointmentDetailTempAsync(model, this.User.Identity.Name);

                }
                catch (DbUpdateConcurrencyException)
                {

                    if (!await _appointmentRespository.ExistAsync(model.PetId))
                    {
                        return new NotFoundViewResult("AppointmentNotFound");
                    }
                    else
                    {
                        throw;
                    }

                }
                return RedirectToAction("Create");
            }

            return View(model);
        }
        public async Task<IActionResult> DeleteAppointment(int? id)
        {
            if (id == null)
            {

                return new NotFoundViewResult("AppointmentNotFound");

            }
            await _appointmentRespository.DeleteDetailTempAsync(id.Value);
            return RedirectToAction("Create");
        }
        public async Task<IActionResult> ConfirmAppointment()
        {

            var response = await _appointmentRespository.ConfirmAppointmentAsync(this.User.Identity.Name);
            if (response != null)
            {
                _mailHelper.SendEmail(response.Pet.Customer.Email,
                 "Appointment Confirmed", $"<h1>Pet Care</h1>" +
             $"Dear {response.Pet.Customer.Name}, " +
                   $"Follow Your Appointment Details</br></br>" +
                   $"Pet:  {response.Pet.Name}</br>" +
                   $"Vet:  {response.Vet.Name}</br>" +
                   $"Date: {response.Date}</br>" +
                   $"Time: {response.Time}</br>" +
                   $"Status: {response.Status}</br>");

            }

            return RedirectToAction("Index");

        }
        [Route("edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {

                return new NotFoundViewResult("AppointmentNotFound");

            }
            var appointmentToEdit = await _appointmentRespository.GetAppointmentByIdAsync(id.Value);

            if (appointmentToEdit == null)
            {
                return new NotFoundViewResult("AppointmentNotFound");

            }
            var model = new AppointmentViewModel
            {
                Id = appointmentToEdit.Id,
                Pets = _petRepository.GetComboPets(),
                Vets = _vetRepository.GetComboVets(),
                Date = appointmentToEdit.Date,
                Time = appointmentToEdit.Time,

            };

            ViewBag.Pets = model.Pets;
            ViewBag.Vets = model.Vets;
            return View(model);
        }
        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> Edit(AppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.Date.Date < DateTime.Now.Date)
                    {
                        _flashMessage.Warning("Date Invalid!");
                        model = new AppointmentViewModel
                        {
                            Pets = _petRepository.GetComboPets(),
                            Vets = _vetRepository.GetComboVets(),
                            Date = DateTime.Now.Date,
                            Time = DateTime.Now.AddHours(8),

                        };
                        ViewBag.Pets = model.Pets;
                        ViewBag.Vets = model.Vets;
                        return View(model);
                    }
                    var appointments = await _appointmentRespository.GetAppointmentAsync(this.User.Identity.Name);
                    bool appointment = appointments.Any(a =>
                    a.Date == model.Date &&
                    a.Time == model.Time &&
                    a.Vet.Id == model.VetId);

                    if (appointment)
                    {
                        _flashMessage.Warning("The vet in this date and time is unvailable");
                        model = new AppointmentViewModel
                        {
                            Pets = _petRepository.GetComboPets(),
                            Vets = _vetRepository.GetComboVets(),
                            Date = DateTime.Now.Date,
                            Time = DateTime.Now.AddHours(8),

                        };
                        ViewBag.Pets = model.Pets;
                        ViewBag.Vets = model.Vets;
                        return View(model);

                    }
                    else
                    {

                        var response = await _appointmentRespository.EditAppointmentAsync(model, this.User.Identity.Name);
                        if (response != null)
                        {
                            _mailHelper.SendEmail(response.Pet.Customer.Email,
                             "Appointment Modified", $"<h1>Pet Care</h1>" +
                         $"Dear {response.Pet.Customer.Name}, " +
                               $"Your appointment was modified! Follow the new details...</br></br>" +
                               $"Pet:  {response.Pet.Name}</br>" +
                               $"Vet:  {response.Vet.Name}</br>" +
                               $"Date: {response.Date.Date}</br>" +
                               $"Time: {response.Time}</br>" +
                               $"Status: {response.Status}</br>");

                        }

                    }



                }
                catch (DbUpdateConcurrencyException)
                {

                    if (!await _appointmentRespository.ExistAsync(model.PetId))
                    {
                        return new NotFoundViewResult("AppointmentNotFound");
                    }
                    else
                    {
                        throw;
                    }

                }
                return RedirectToAction("Index");
            }

            return View(model);
        }
        public async Task<IActionResult> CancelAppointment(int id)
        {
            var response = await _appointmentRespository.CancelAppointmentAsync(id);
            if (response != null)
            {
                _mailHelper.SendEmail(response.Pet.Customer.Email,
                 "Appointment Cancelled", $"<h1>Pet Care</h1>" +
             $"Dear {response.Pet.Customer.Name}, " +
                   $"Your appointment was cancelled! Follow the details...</br></br>" +
                   $"Pet:  {response.Pet.Name}</br>" +
                   $"Vet:  {response.Vet.Name}</br>" +
                   $"Date: {response.Date.Date}</br>" +
                   $"Time: {response.Time}</br>" +
                   $"Status: {response.Status}</br>");

            }

            return RedirectToAction("Index");

        }

        [Route("concludeappointment")]
        public async Task<IActionResult> ConcludeAppointment(int? id)
        {
            if (id == null)
            {

                return new NotFoundViewResult("AppointmentNotFound");

            }
            var appointment = await _appointmentRespository.GetAppointmentByIdAsync(id.Value);

            if (appointment == null)
            {
                return new NotFoundViewResult("AppointmentNotFound");

            }
            var model = new BillViewModel
            {
                Appointments = _appointmentRespository.GetComboAppointments()

            };

            ViewBag.Appointments = model.Appointments;

            return View(model);

        }

        [HttpPost]
        [Route("concludeappointment")]
        public async Task<IActionResult> ConcludeAppointment(BillViewModel model)
        {
            var response = await _appointmentRespository.ConcludeAppointmentAsync(model);
            if (response != null)
            {
                _mailHelper.SendEmail(response.Pet.Customer.Email,
                 "Appointment Concluded...", $"<h1>Pet Care</h1>" +
             $"Dear {response.Pet.Customer.Name}, " +
                   $"Your appointment was concluded ! Please Follow your Bill...</br></br>" +
                   $"Pet:  {response.Pet.Name}</br>" +
                   $"Vet:  {response.Vet.Name}</br>" +
                   $"Date: {response.Date.Date}</br>" +
                   $"Time: {response.Time}</br>" +
                   $"Status: {response.Status}</br>");

            }



            return RedirectToAction("Index");
        }

        
        [Route("appointmentnotfound")]
        public IActionResult AppointmentNotFound()
        {
            return View();
        }
       




    }
}
