using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VeterinaryClinic.Repositories;

namespace VeterinaryClinic.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetReportController : Controller
    {
        private readonly IPetReportRepository _petReportRepository;

        public PetReportController(IPetReportRepository petReportRepository)
        {
            _petReportRepository = petReportRepository;
        }
        public IActionResult GetPetReport()
        {
            return Ok(_petReportRepository.GetAll()
                .Include(r => r.Pet)
                .ThenInclude(r => r.Customer)
                .Include(r => r.Vet)
                .ThenInclude(r => r.User));
        }
    }
}
