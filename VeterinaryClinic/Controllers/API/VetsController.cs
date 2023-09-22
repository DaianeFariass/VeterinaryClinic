using Microsoft.AspNetCore.Mvc;
using VeterinaryClinic.Repositories;

namespace VeterinaryClinic.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class VetsController : Controller
    {
        private readonly IVetRepository _vetRepository;

        public VetsController(IVetRepository vetRepository)
        {
            _vetRepository = vetRepository;
        }
        [HttpGet]
        public IActionResult GetVets()
        {
            return Ok(_vetRepository.GetAllWithUsers());

        }
    }
}
