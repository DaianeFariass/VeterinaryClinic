using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using System;
using System.Linq;
using System.Threading.Tasks;
using VeterinaryClinic.Data;
using VeterinaryClinic.Data.Entities;
using VeterinaryClinic.Helpers;
using VeterinaryClinic.Migrations;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Repositories
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRespository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public AppointmentRepository(DataContext context, IUserHelper userHelper) : base(context) 
        {
             _context = context;
             _userHelper = userHelper;
        }

        public async Task AddItemToAppointmenteAsync(AppointmentViewModel model, string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return;

            }
            var pet = await _context.Pets.FindAsync(model.PetId);
            if (pet == null)
            {
                return;
            }
            var vet = await _context.Vets.FindAsync(model.VetId);
            if (vet == null)
            {
                return;

            }
            var appointmentDetailTemp = await _context.AppointmentDetailsTemp
                .Where(adt => adt.User == user && adt.Pet == pet && adt.Vet == vet)
                .FirstOrDefaultAsync();

            if (appointmentDetailTemp == null)
            {
                appointmentDetailTemp = new AppointmentDetailTemp
                {
                    Pet = pet,
                    Vet = vet,
                    Date = model.Date,
                    Time = model.Time,
                    User = user
                };
                _context.AppointmentDetailsTemp.Add(appointmentDetailTemp);
            }
            else 
            { 
                _context.AppointmentDetailsTemp.Update(appointmentDetailTemp);
            
            }
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ConfirmAppointmentAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if(user == null)
            {
                return false;
            }
            var appointmentTmps = await _context.AppointmentDetailsTemp
                .Include(p => p.Pet)
                .Include(v => v.Vet)
                .Where(a => a.User == user)
                .ToListAsync();
            if(appointmentTmps == null || appointmentTmps.Count == 0)
            {
                return false;
            }
            var details = appointmentTmps.Select(a => new AppointmentDetail
            {
                Pet = a.Pet,
                Vet = a.Vet,
                Date = a.Date,
                Time = a.Time

            }).ToList();
            var appointment = new Appointment
            {
                Date = DateTime.UtcNow,
                User = user,
                Items = details
            };
            await CreateAsync(appointment);
            _context.AppointmentDetailsTemp.RemoveRange(appointmentTmps);
            await _context.SaveChangesAsync();
            return true;
            
        }

        public async Task DeleteDetailTempAsync(int id)
        {
            var appointmentDetailTemp = await _context.AppointmentDetailsTemp.FindAsync(id);
            if(appointmentDetailTemp == null) 
            {
                return;
            
            }
            _context.AppointmentDetailsTemp.Remove(appointmentDetailTemp);
            await _context.SaveChangesAsync();
            
        }

        public async Task<IQueryable<Appointment>> GetApointmentAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null) 
            { 
                return null;
            
            }
            if (await _userHelper.IsUserInRoleAsync(user, "Customer") || (await _userHelper.IsUserInRoleAsync(user, "Vet")))
            {
                return _context.Appointments
                    .Include(a => a.User)
                    .Include(a => a.Items)
                    .OrderByDescending(a => a.Date);
            }
            return _context.Appointments
                .Include(a => a.User)
                .Include(a => a.Items)
                .Where(a => a.User == user)
                .OrderByDescending(a => a.Date);

        }

        public async Task<IQueryable<AppointmentDetailTemp>> GetDetailsTempsAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return null;

            }
            return _context.AppointmentDetailsTemp
                .Include(p => p.Pet)
                .Include(v => v.Vet)
                .Where(a => a.User == user)
                .OrderByDescending(a => a.Pet.Name);

        }

        public async Task ModifyAppointmentDetailAsync(int id)
        {
            var appointmentDetailTemp = await _context.AppointmentDetailsTemp.FindAsync(id);
            if (appointmentDetailTemp == null) 
            { 
                return;
            
            }
            if(appointmentDetailTemp.Id > 0)
            {
                _context.AppointmentDetailsTemp.Update(appointmentDetailTemp);
                await _context.SaveChangesAsync();
            }
            
        }
        public async Task DeleteAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment== null)
            {
                return;

            }
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

        }

        public async Task<AppointmentDetailTemp> GetAppointmentDetailTempAsync(int id)
        {
           return await _context.AppointmentDetailsTemp.FindAsync(id);
               
        }

        public async Task EditAppointmentDetailTempAsync(EditAppointmentDetailTempViewModel model)
        {
            var appointmentDetailTemp = await _context.AppointmentDetailsTemp.FindAsync(model);
            if(appointmentDetailTemp == null) 
            { 
                return;      
            }
            appointmentDetailTemp = new AppointmentDetailTemp
            {
                
                Date = model.Date,
                Time = model.Time,
               
            };
            _context.AppointmentDetailsTemp.Update(appointmentDetailTemp);
            await _context.SaveChangesAsync();
            
        }
    }
}
