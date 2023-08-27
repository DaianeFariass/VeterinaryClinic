﻿using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vereyon.Web;
using VeterinaryClinic.Data;
using VeterinaryClinic.Data.Entities;
using VeterinaryClinic.Helpers;
using VeterinaryClinic.Migrations;
using VeterinaryClinic.Models;
using static System.Net.Mime.MediaTypeNames;

namespace VeterinaryClinic.Repositories
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRespository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;


        public AppointmentRepository(DataContext context,
            IUserHelper userHelper) : base(context)
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

            var appointment = new Appointment
            {
                Date = DateTime.UtcNow,
                Time = DateTime.UtcNow,
                User = user,
                Items = details
            };
            await CreateAsync(appointment);
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

        public async Task EditAppointmentDetailTempAsync(AppointmentViewModel model, string username)
        {
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
            var appointmentDetailTemp = await _context.AppointmentDetailsTemp.FindAsync(model.Id);

            appointmentDetailTemp.User = user;
            appointmentDetailTemp.Pet = pet;
            appointmentDetailTemp.Vet = vet;
            appointmentDetailTemp.Date = model.Date;
            appointmentDetailTemp.Time = model.Time;

            _context.AppointmentDetailsTemp.Update(appointmentDetailTemp);

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
        

            var notification = new Notifications
            {
                Appointment = appointment,
                NotificationTypes = notificationTypes,
                IsRead = false,

            };

            _context.Notifications.Add(notification);

            await _context.SaveChangesAsync();


        }

        public IQueryable GetNotificationsAsync()
        {

            return _context.Notifications
                .Include(n => n.Appointment);
                
        }
    }
}
