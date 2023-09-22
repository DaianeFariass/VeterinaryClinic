using Microsoft.AspNetCore.Mvc;
using VeterinaryClinic.Repositories;

namespace VeterinaryClinic.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PetsController : Controller
    {
        private readonly IPetRepository _petRepository;

        public PetsController(IPetRepository petRepository)
        {
            _petRepository = petRepository;
        }
        [HttpGet]
        public IActionResult GetPets()
        {
            return Ok(_petRepository.GetAllWithCustomers());
        }
    }
}
