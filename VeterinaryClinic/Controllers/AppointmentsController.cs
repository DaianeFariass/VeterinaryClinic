using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
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

        public AppointmentsController(IAppointmentRespository appointmentRespository,
            IPetRepository petRepository,
            IVetRepository vetRepository)
        {
            _appointmentRespository = appointmentRespository;
            _petRepository = petRepository;
            _vetRepository = vetRepository;
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
            var model = new AddItemViewModel
            {
                Pets = _petRepository.GetComboPets(),
                Vets = _vetRepository.GetComboVets()

            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddAppointment(AddItemViewModel model)
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
            var appointment = await _appointmentRespository.GetByIdAsync(id.Value);
           
            return View(appointment);
        }
        [HttpPost]
        public async Task<IActionResult> EditAppointment(AddItemViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    

                }
                catch (System.Exception)
                {

                    throw;
                }
            }

            return View();
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

    }
}
