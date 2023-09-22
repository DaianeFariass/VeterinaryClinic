using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        private readonly IMailHelper _mailHelper;

        public AppointmentRepository(
            DataContext context,
            IUserHelper userHelper,
            IConverterHelper converterHelper,
            IMailHelper mailHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
            _mailHelper = mailHelper;
        }

        /// <summary>
        /// Método para criar um  appointmentDetailTemp.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userName"></param>
        /// <returns>AppointmentDetailTemp</returns>
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
        /// <summary>
        /// Método que confirma o appointment passando da tabela appointmentDetailTemp para appointmentDetais e Appointments.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>AppointmentDetail, Appointment</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Appointment> ConfirmAppointmentAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                throw new NotImplementedException();
            }
            var appointmentTmps = await _context.AppointmentDetailsTemp
                .Include(p => p.Pet)
                .ThenInclude(p => p.Customer)
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
                    Date = appointmentDetail.Date,
                    Time = appointmentDetail.Time,
                    Status = StatusAppointment.Confirmed,


                };

                await CreateAsync(appointment);
                await SendAppointmentNotification(appointment, user.UserName, NotificationTypes.Create);
            }

            _context.AppointmentDetailsTemp.RemoveRange(appointmentTmps);
            await _context.SaveChangesAsync();
            return appointment;

        }
        /// <summary>
        /// Método que deleta o appointmentDetailTemp.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Delete</returns>
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
        /// <summary>
        /// Método que retorna um appointment através do user.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>Appointment</returns>
        public async Task<IQueryable<Appointment>> GetAppointmentAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return null;

            }
            if (await _userHelper.IsUserInRoleAsync(user, "Receptionist") || (await _userHelper.IsUserInRoleAsync(user, "Vet")))
            {
                return _context.Appointments
                    .Include(a => a.User)
                    .Include(a => a.Pet)
                    .Include(a => a.Vet)
                    .OrderBy(a => a.Pet.Name);
            }
            if (await _userHelper.IsUserInRoleAsync(user, "Customer") || (await _userHelper.IsUserInRoleAsync(user, "Admin")))
            {
                return _context.Appointments
                    .Include(a => a.Pet)
                    .Include(a => a.Vet)
                    .OrderBy(a => a.Pet.Name);
            }
            return _context.Appointments
                .Include(a => a.User)
                .Where(a => a.User == user)
                .OrderBy(a => a.Pet.Name);

        }
        /// <summary>
        /// Método que retorna um appointment por id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Appointment</returns>
        public async Task<Appointment> GetAppointmentByIdAsync(int id)
        {
            return await _context.Appointments.FindAsync(id);
        }
        /// <summary>
        /// Método que retorna um appointmentDetailTemp pelo user.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>AppointmentDetailTemp</returns>
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
        /// <summary>
        /// Método que modifica o appointmentDetailTemp.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Update</returns>
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
        /// <summary>
        /// Método que retorna o appointmentDetailTemp por id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>AppointmentDetailTemp</returns>
        public async Task<AppointmentDetailTemp> GetAppointmentDetailTempAsync(int id)
        {
            return await _context.AppointmentDetailsTemp.FindAsync(id);

        }
        /// <summary>
        /// Método para editar o AppointmentDetailTemp.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="username"></param>
        /// <returns>Edit</returns>
        public async Task EditAppointmentDetailTempAsync(AppointmentDetailsViewModel model, string username)
        {
            _converterHelper.ToAppointmentDetailTemp(model, false);

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
            var appointment = new AppointmentDetailsViewModel
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

            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// Método que retorna o appointment através do user.
        /// </summary>
        /// <returns>appointment</returns>
        public IQueryable GetAppointmentsWithUser()
        {
            return _context.Appointments.Include(a => a.User);
        }
        /// <summary>
        /// Método que retorna AppointmentDetails.
        /// </summary>
        /// <returns>AppointmentsDetails</returns>
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
            var hasVetRole = await _userHelper.IsUserInRoleAsync(user, "Vet");
            var hasReceptionistRole = await _userHelper.IsUserInRoleAsync(user, "Receptionist");


            if (hasCustomerRole == false && hasReceptionistRole == false && hasVetRole == false)
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


        public async Task<Appointment> CancelAppointmentAsync(int id)
        {

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return null;

            }
            var customers = _context.Appointments
            .Include(a => a.Pet)
            .ThenInclude(a => a.Customer)
            .Include(a => a.Vet)
            .Include(a => a.User)
            .ToList();
            var details = customers.Select(a => new AppointmentViewModel
            {
                Pet = appointment.Pet,
                Vet = appointment.Vet,
                User = appointment.User,

            });

            appointment.Status = StatusAppointment.Cancelled;
            _context.Appointments.Update(appointment);
            await SendAppointmentNotification(appointment, appointment.User.UserName, NotificationTypes.Cancel);
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<Appointment> ConcludeAppointmentAsync(BillViewModel model)
        {
            var appointment = await _context.Appointments.FindAsync(model.Id);
            if (appointment == null)
            {
                return null;

            }
            var appointments = _context.Appointments
           .Include(a => a.Pet)
           .ThenInclude(a => a.Customer)
           .Include(a => a.Vet)
           .Include(a => a.User)
           .ToList();
            var details = appointments.Select(a => new BillViewModel
            {

                User = appointment.User,

            });
            appointment.Status = StatusAppointment.Concluded;
            await SendAppointmentBill(appointment, appointment.User.UserName);
            await SendAppointmentNotification(appointment, appointment.User.UserName, NotificationTypes.Conclude);
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();

            return appointment;
        }

        public async Task<Appointment> EditAppointmentAsync(AppointmentViewModel model, string username)
        {
            _converterHelper.ToAppointment(model, false);

            var user = await _userHelper.GetUserByEmailAsync(username);
            if (user == null)
            {
                throw new NotImplementedException();

            }
            var pet = await _context.Pets.FindAsync(model.PetId);
            if (pet == null)
            {
                throw new NotImplementedException();
            }
            var customers = _context.Pets
             .Include(p => p.Customer)
             .Where(p => p.Customer.User == user)
             .ToList();
            var details = customers.Select(a => new AppointmentViewModel
            {
                Pet = pet,

            });

            var vet = await _context.Vets.FindAsync(model.VetId);
            if (vet == null)
            {
                throw new NotImplementedException();
            }
            var appointment = new AppointmentViewModel
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
            _context.Appointments.Update(appointment);
            await SendAppointmentNotification(appointment, user.UserName, NotificationTypes.Edit);
            await _context.SaveChangesAsync();
            return appointment;

        }
        public async Task SendAppointmentBill(Appointment appointment, string username)
        {
            var user = await _userHelper.GetUserByEmailAsync(username);
            if (user == null)
            {
                return;

            }
            var hasCustomerRole = await _userHelper.IsUserInRoleAsync(user, "Customer");
            var hasVetRole = await _userHelper.IsUserInRoleAsync(user, "Vet");
            var hasReceptionistRole = await _userHelper.IsUserInRoleAsync(user, "Receptionist");


            if (hasCustomerRole == false && hasReceptionistRole == false && hasVetRole == false)
            {
                return;
            }

            var bill = new Bill
            {
                Appointment = appointment,
                User = user,
                Cost = 60
            };

            _context.Bills.Add(bill);

            await _context.SaveChangesAsync();


        }

        public IEnumerable<SelectListItem> GetComboAppointments()
        {
            var list = _context.Appointments.Select(p => new SelectListItem
            {
                Text = p.Pet.Customer.Name,
                Value = p.Id.ToString(),

            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select the Customer...)",
                Value = "0"
            });

            return list;

        }
    }
}
