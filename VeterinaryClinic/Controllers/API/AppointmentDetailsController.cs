using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VeterinaryClinic.Repositories;

namespace VeterinaryClinic.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentDetailsController : Controller
    {
        private readonly IAppointmentRespository _appointmentRespository;

        public AppointmentDetailsController(IAppointmentRespository appointmentRespository)
        {
            _appointmentRespository = appointmentRespository;
        }
        public IActionResult GetAppointmentsDetails()
        {
            return Ok(_appointmentRespository.GetAppointmentsDetails());
        }
    }
}
