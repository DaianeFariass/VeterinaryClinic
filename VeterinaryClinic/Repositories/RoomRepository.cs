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

        public RoomRepository(DataContext context,
            IUserHelper userHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
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
            list.Insert(1, new SelectListItem
            {
                Text = "(Available)",
                Value = "1",

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
       
                var roomIndex = new RoomViewModel
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
            var user = await _userHelper.GetUserByEmailAsync(username);
            if (user == null)
            {
                return;

            }
            var vet = await _context.Vets.FindAsync(model.VetId);
            if (vet == null)
            {
                return;
            }
            var room= await _context.Rooms
                .Where(r => r.Vet.Id == vet.Id)
                .FirstOrDefaultAsync();


            if (room == null)
            {
                room = await _context.Rooms
                    .Where(r => r.RoomNumber == model.RoomNumber)
                    .FirstOrDefaultAsync();

                room.Vet = vet;
                _context.Rooms.Update(room);

            }
            await _context.SaveChangesAsync();

        }


    }
}
