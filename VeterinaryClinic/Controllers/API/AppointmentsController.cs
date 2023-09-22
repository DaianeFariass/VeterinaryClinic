using Microsoft.AspNetCore.Mvc;
using VeterinaryClinic.Repositories;

namespace VeterinaryClinic.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentRespository _appointmentRespository;

        public AppointmentsController(IAppointmentRespository appointmentRespository)
        {
            _appointmentRespository = appointmentRespository;
        }
        public IActionResult GetAppointments()
        {
            return Ok(_appointmentRespository.GetAppointmentsWithUser());
        }
    }
}
