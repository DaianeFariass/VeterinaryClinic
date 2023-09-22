using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VeterinaryClinic.Data.Entities;
using VeterinaryClinic.Enums;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Repositories
{
    public interface IAppointmentRespository : IGenericRepository<Appointment>
    {

        Task AddItemToAppointmenteAsync(AppointmentDetailsViewModel model, string userName);

        Task<Appointment> ConfirmAppointmentAsync(string userName);

        Task<Appointment> CancelAppointmentAsync(int id);

        Task<Appointment> ConcludeAppointmentAsync(BillViewModel model);

        Task DeleteDetailTempAsync(int id);

        Task EditAppointmentDetailTempAsync(AppointmentDetailsViewModel model, string username);

        Task<Appointment> EditAppointmentAsync(AppointmentViewModel model, string username);

        Task<Appointment> GetAppointmentByIdAsync(int id);

        Task<IQueryable<Appointment>> GetAppointmentAsync(string userName);

        public IQueryable GetAppointmentsWithUser();

        public IQueryable GetAppointmentsDetails();

        public IQueryable GetAppointmentsDetailsTemp();

        Task<AppointmentDetailTemp> GetAppointmentDetailTempAsync(int id);

        Task<IQueryable<AppointmentDetailTemp>> GetDetailsTempsAsync(string userName);

        Task ModifyAppointmentDetailAsync(int id);

        Task SendAppointmentNotification(Appointment appointment, string username, NotificationTypes notificationTypes);

        Task SendAppointmentBill(Appointment appointment, string username);

        IEnumerable<SelectListItem> GetComboAppointments();
    }
}
