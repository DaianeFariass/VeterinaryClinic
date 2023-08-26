using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VeterinaryClinic.Repositories;

namespace VeterinaryClinic.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomRepository _roomRepository;

        public RoomsController(IRoomRepository roomRepository)
        {
           _roomRepository = roomRepository;
        }
        public IActionResult GetRooms()
        {
            return Ok(_roomRepository.GetAll().Include(r => r.Vet).Include(r => r.User));
        }
    }
}
