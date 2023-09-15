using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using VeterinaryClinic.Data;
using VeterinaryClinic.Data.Entities;
using VeterinaryClinic.Enums;
using VeterinaryClinic.Helpers;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Repositories
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRespository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;

        public AppointmentRepository(
            DataContext context,
            IUserHelper userHelper,
            IConverterHelper converterHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
        }


        public async Task AddItemToAppointmenteAsync(AppointmentDetailsViewModel model, string userName)
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

        public async Task<Appointment> ConfirmAppointmentAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                throw new NotImplementedException();
            }
            var appointmentTmps = await _context.AppointmentDetailsTemp
                .Include(p => p.Pet)
                .Include(v => v.Vet)
                .Where(a => a.User == user)
                .ToListAsync();
            if (appointmentTmps == null || appointmentTmps.Count == 0)
            {
                throw new NotImplementedException();
            }
            var details = appointmentTmps.Select(a => new AppointmentDetail
            {
                Pet = a.Pet,
                Vet = a.Vet,
                Date = a.Date,
                Time = a.Time

            }).ToList();
            var appointment = new Appointment();
            foreach (AppointmentDetail appointmentDetail in details)
            {
                appointment = new Appointment
                {
                    User = user,
                    Pet = appointmentDetail.Pet,
                    Vet = appointmentDetail.Vet,
                    Date = DateTime.UtcNow,
                    Time = DateTime.UtcNow,
                    Status = StatusAppointment.Confirmed,

                };
                await CreateAsync(appointment);


            }

            _context.AppointmentDetailsTemp.RemoveRange(appointmentTmps);
            await _context.SaveChangesAsync();
            return appointment;

        }

        public async Task DeleteDetailTempAsync(int id)
        {
            var appointmentDetailTemp = await _context.AppointmentDetailsTemp.FindAsync(id);
            if (appointmentDetailTemp == null)
            {
                return;

            }
            _context.AppointmentDetailsTemp.Remove(appointmentDetailTemp);
            await _context.SaveChangesAsync();

        }

        public async Task<IQueryable<Appointment>> GetAppointmentAsync(string userName)
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
                    .Include(a => a.Pet)
                    .Include(a => a.Vet)
                    .OrderByDescending(a => a.Date);
            }
            return _context.Appointments
                .Include(a => a.User)
                .Where(a => a.User == user)
                .OrderByDescending(a => a.Date);

        }
        public async Task<Appointment> GetAppointmentByIdAsync(int id)
        {
            return await _context.Appointments.FindAsync(id);
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
            if (appointmentDetailTemp.Id > 0)
            {
                _context.AppointmentDetailsTemp.Update(appointmentDetailTemp);
                await _context.SaveChangesAsync();
            }

        }

        public async Task<AppointmentDetailTemp> GetAppointmentDetailTempAsync(int id)
        {
            return await _context.AppointmentDetailsTemp.FindAsync(id);

        }

        public async Task EditAppointmentDetailTempAsync(AppointmentDetailsViewModel model, string username)
        {
            _converterHelper.ToAppointmentDetailTemp(model,false);

            var user = await _userHelper.GetUserByEmailAsync(username);
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
            
            var appointment = await _context.AppointmentDetailsTemp
            .Where(a => a.User == user && a.Pet.Id == pet.Id && a.Vet.Id == vet.Id)
            .FirstOrDefaultAsync();

            if (appointment == null)
            {
                appointment = new AppointmentDetailsViewModel
                {
                    Id = model.Id,
                    User = user,
                    Pet = pet,
                    PetId = pet.Id,
                    Vet = vet,
                    VetId = vet.Id,
                    Date = model.Date,
                    Time = model.Time,
                };
                _context.AppointmentDetailsTemp.Update(appointment);

            }

            await _context.SaveChangesAsync();



        }

        public IQueryable GetAppointmentsWithUser()
        {
            return _context.Appointments.Include(a => a.User);
        }

        public IQueryable GetAppointmentsDetails()
        {
            return _context.AppointmentDetails
                .Include(a => a.Pet)
                .ThenInclude(a => a.Customer)
                .ThenInclude(a => a.User)
                .Include(a => a.Vet)
                .ThenInclude(a => a.User);


        }

        public IQueryable GetAppointmentsDetailsTemp()
        {
            return _context.AppointmentDetailsTemp
                .Include(a => a.Pet)
                .ThenInclude(a => a.Customer)
                .ThenInclude(a => a.User)
                .Include(a => a.Vet)
                .ThenInclude(a => a.User);
        }

        public async Task SendAppointmentNotification(Appointment appointment, string username, NotificationTypes notificationTypes)
        {
            var user = await _userHelper.GetUserByEmailAsync(username);
            if (user == null)
            {
                return;

            }
            var hasCustomerRole = await _userHelper.IsUserInRoleAsync(user, "Customer");
            var hasReceptionistRole = await _userHelper.IsUserInRoleAsync(user, "Receptionist");

            if (hasCustomerRole == false && hasReceptionistRole == false)
            {
                return;
            }


            var notification = new Notification
            {
                Appointment = appointment,
                NotificationTypes = notificationTypes,

            };

            _context.Notifications.Add(notification);

            await _context.SaveChangesAsync();


        }

        public IQueryable GetNotificationsAsync()
        {

            return _context.Notifications
                .Include(n => n.Appointment);

        }

        public async Task<Appointment> CancelAppointmentAsync(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return null;

            }
            else
            {
                appointment.Status = StatusAppointment.Cancelled;
                _context.Appointments.Update(appointment);
                await _context.SaveChangesAsync();
            }
            return appointment;
        }

        public async Task<Appointment> ConcludeAppointmentAsync(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return null;

            }
            else
            {
                appointment.Status = StatusAppointment.Concluded;
                _context.Appointments.Update(appointment);
                await _context.SaveChangesAsync();
            }
            return appointment;
        }

        public async Task EditAppointmentAsync(AppointmentViewModel model, string username)
        {
            _converterHelper.ToAppointment(model, false);
            var user = await _userHelper.GetUserByEmailAsync(username);
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
            var appointment  = await _context.Appointments
            .Where(a => a.User == user && a.Pet.Id == pet.Id && a.Vet.Id == vet.Id )
            .FirstOrDefaultAsync();

            if (appointment == null)
            {
                appointment = new AppointmentViewModel
                {
                    User = user,
                    Pet = pet,
                    PetId = pet.Id,
                    Vet = vet,
                    VetId = vet.Id,
                    Date = model.Date,
                    Time = model.Time,
                };
                _context.Appointments.Update(appointment);

            }

            await _context.SaveChangesAsync();
            
        }
    }
}
