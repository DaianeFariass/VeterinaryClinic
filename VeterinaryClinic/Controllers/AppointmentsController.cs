using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
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
        private readonly IConverterHelper _converter;

        public AppointmentsController(IAppointmentRespository appointmentRespository,
            IPetRepository petRepository,
            IVetRepository vetRepository,
            IConverterHelper converter)
        {
            _appointmentRespository = appointmentRespository;
            _petRepository = petRepository;
            _vetRepository = vetRepository;
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
                Date= DateTime.Now,
       
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddAppointment(AppointmentViewModel model)
        {
            if(ModelState.IsValid) 
            { 
                await _appointmentRespository.AddItemToAppointmenteAsync(model, this.User.Identity.Name);
                return RedirectToAction("Create");
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
            var model = new EditAppointmentDetailTempViewModel
            {
                Id = appointmentToEdit.Id,
                Pet= appointmentToEdit.Pet,
                Vet= appointmentToEdit.Vet,             
                User = appointmentToEdit.User,
                Pets = _petRepository.GetComboPets(),
                Vets = _vetRepository.GetComboVets(),
                Date = DateTime.Now.Date,
                Time= DateTime.Now.AddHours(8),

            };
            
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditAppointment(EditAppointmentDetailTempViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    await _appointmentRespository.EditAppointmentDetailTempAsync(model);
                    return RedirectToAction("Index");

                }
                catch (DbUpdateConcurrencyException)
                {

                    //if (!await _appointmentRespository.ExistAsync())
                    //{
                    //    return new NotFoundViewResult("AppointmentNotFound");
                    //}
                    //else
                    //{
                    //    throw;
                    //}
                   
                }
            
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
            return RedirectToAction("Criate");
        }
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id == null)
            {

                return NotFound();

            }
            await _appointmentRespository.DeleteAppointment(id.Value);
            return RedirectToAction("Create");

        }

    }
}
