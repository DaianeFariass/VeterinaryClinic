using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VeterinaryClinic.Data.Entities;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Repositories
{
    public interface IRoomRepository : IGenericRepository<Room>
    {
        public IQueryable GetAllWithVets();

        IEnumerable<SelectListItem> GetComboVets();

        IEnumerable<SelectListItem> GetComboTypes();

        public Task AddVetToRoomAsync(RoomViewModel model, string userName);

        public Task<IQueryable<Room>> GetRoomAsync(string userName);

        public Task EditRoomAsync(RoomViewModel model, string username);

    }
}
