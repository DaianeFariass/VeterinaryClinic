using Microsoft.AspNetCore.Mvc;
using VeterinaryClinic.Repositories;

namespace VeterinaryClinic.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentDetailsTempController : Controller
    {
        private readonly IAppointmentRespository _appointmentRespository;

        public AppointmentDetailsTempController(IAppointmentRespository appointmentRespository)
        {
            _appointmentRespository = appointmentRespository;
        }
        public IActionResult GetAppointmentsDetailsTemp()
        {
            return Ok(_appointmentRespository.GetAppointmentsDetailsTemp());
        }
    }
}
