using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VeterinaryClinic.Repositories;

namespace VeterinaryClinic.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentRespository _appointmentRespository;

        public AppointmentsController(IAppointmentRespository appointmentRespository)
        {
            _appointmentRespository = appointmentRespository;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _appointmentRespository.GetApointmentAsync(this.User.Identity.Name);
            return View(model);
        }
    }
}
