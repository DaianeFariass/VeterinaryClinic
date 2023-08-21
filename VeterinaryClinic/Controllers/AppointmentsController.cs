using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vereyon.Web;
using VeterinaryClinic.Data.Entities;
using VeterinaryClinic.Helpers;
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
        private readonly IFlashMessage _flashMessage;
        private readonly IConverterHelper _converter;

        public AppointmentsController(IAppointmentRespository appointmentRespository,
            IPetRepository petRepository,
            IVetRepository vetRepository,
            IFlashMessage flashMessage,
            IConverterHelper converter)
        {
            _appointmentRespository = appointmentRespository;
            _petRepository = petRepository;
            _vetRepository = vetRepository;
            _flashMessage = flashMessage;
            _converter = converter;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _appointmentRespository.GetApointmentAsync(this.User.Identity.Name);
            return View(model);
        }
        public async Task<IActionResult> Create()
        {
            var model = await _appointmentRespository.GetDetailsTempsAsync(this.User.Identity.Name);
            return View(model);
        }
        public IActionResult AddAppointment()
        {
            
            var model = new AppointmentViewModel
            {

                Pets = _petRepository.GetComboPets(),
                Vets = _vetRepository.GetComboVets(),
                Date = DateTime.Now.Date,
                Time = DateTime.Now.AddHours(8),
                
       
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddAppointment(AppointmentViewModel model)
        {
            if(ModelState.IsValid) 
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
                    model = new AppointmentViewModel
                    {
                        Pets = _petRepository.GetComboPets(),
                        Vets = _vetRepository.GetComboVets(),
                        Date = DateTime.Now.Date,
                        Time = DateTime.Now.AddHours(8),

                    };
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
        public async Task<IActionResult> EditAppointment(int? id)
        {
            if (id == null)
            {

                return NotFound();

            }
            var appointmentToEdit = await _appointmentRespository.GetAppointmentDetailTempAsync(id.Value);

            if(appointmentToEdit == null) 
            {
                return NotFound();
            
            }
            var model = new AppointmentViewModel
            {
                Pets = _petRepository.GetComboPets(),
                Vets = _vetRepository.GetComboVets(),
                Date = DateTime.Now.Date,
                Time = DateTime.Now.AddHours(8),
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditAppointment(AppointmentViewModel model)
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
                        model = new AppointmentViewModel
                        {
                            Pets = _petRepository.GetComboPets(),
                            Vets = _vetRepository.GetComboVets(),
                            Date = DateTime.Now.Date,
                            Time = DateTime.Now.AddHours(8),

                        };
                        return View(model);

                    }
                    else
                    {
                        
                        await _appointmentRespository.EditAppointmentDetailTempAsync(model, this.User.Identity.Name);
                      

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
                return RedirectToAction("Create");
            }

            return View(model);
        }
        public async Task<IActionResult> DeleteAppointment(int? id)
        {
            if(id == null) 
            { 
            
                return NotFound();
            
            }
            await _appointmentRespository.DeleteDetailTempAsync(id.Value);
            return RedirectToAction("Create");
        }
        public async Task<IActionResult> ConfirmAppointment()
        {
            var response = await _appointmentRespository.ConfirmAppointmentAsync(this.User.Identity.Name);  
            if(response)
            {
                return RedirectToAction("Index");

            }
            return RedirectToAction("Create");
        }
     

    }
}
