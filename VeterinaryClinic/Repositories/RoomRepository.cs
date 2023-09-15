using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VeterinaryClinic.Data;
using VeterinaryClinic.Data.Entities;
using VeterinaryClinic.Helpers;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Repositories
{
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;

        public RoomRepository(DataContext context,
            IUserHelper userHelper,
            IConverterHelper converterHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
        }

        public IQueryable GetAllWithVets()
        {
            return _context.Rooms.Include(p => p.Vet);
        }
        public IEnumerable<SelectListItem> GetComboVets()
        {
            var list = _context.Vets.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),

            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select the Veterinary...)",
                Value = "0",

            });

            return list;
        }
        public IEnumerable<SelectListItem> GetComboTypes()
        {
            var model = new RoomViewModel
            {
                Types = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Select the room type...", Value = "" },
                    new SelectListItem { Text = "Radiology", Value = "Radiology" },
                    new SelectListItem { Text = "Cardiology", Value = "Cardiology" },
                    new SelectListItem { Text = "Dermatology", Value = "Dermatology" },
                    new SelectListItem { Text = "Ophthalmology", Value = "Ophthalmology" },
                    new SelectListItem { Text = "Surgery", Value = "Surgery" },
                    new SelectListItem { Text = "UTI", Value = "UTI" },

                }
            };
            return model.Types;

        }
        public async Task AddVetToRoomAsync(RoomViewModel model, string userName)
        {

            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return;

            }
            var vet = await _context.Vets.FindAsync(model.VetId);

            var roomIndex = new Room
            {
                RoomNumber = model.RoomNumber,
                Type = model.TypeId.ToString(),
                Status = model.Status,
                Vet = vet,
                User = user
            };
            _context.Rooms.Add(roomIndex);


            await _context.SaveChangesAsync();
        }
        public async Task<IQueryable<Room>> GetRoomAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return null;

            }
            return _context.Rooms
                  .Include(r => r.Vet)
                  .Where(r => r.User == user)
                  .OrderByDescending(a => a.RoomNumber);

        }
        public async Task EditRoomAsync(RoomViewModel model, string username)
        {
            _converterHelper.ToRoom(model, false);

            var user = await _userHelper.GetUserByEmailAsync(username);
            if (user == null)
            {
                return;

            }
            var vet = await _context.Vets.FindAsync(model.VetId);
            if (vet == null) 
            {
              
                var roomIndex = new RoomViewModel
                {
                    Id = model.Id,
                    User = user,
                    RoomNumber = model.RoomNumber,
                    Type = model.TypeId.ToString(),
                    TypeId = model.TypeId,
                    Vet = model.Vet,
                    VetId = model.VetId,
                    Status = model.Status,

                };
                _context.Rooms.Update(roomIndex);
                var room = _context.Rooms.FindAsync(model.Id);
                 room.Result.Vet = vet;
                _context.Rooms.Update(room.Result);

            }
            else
            {
                var roomIndex = new RoomViewModel
                {
                    Id = model.Id,
                    User = user,
                    RoomNumber = model.RoomNumber,
                    Type = model.TypeId.ToString(),
                    TypeId = model.TypeId,
                    Vet = vet,
                    VetId = vet.Id,
                    Status = model.Status,

                };
                _context.Rooms.Update(roomIndex);
            }
        

            await _context.SaveChangesAsync();

        }

    }
}
