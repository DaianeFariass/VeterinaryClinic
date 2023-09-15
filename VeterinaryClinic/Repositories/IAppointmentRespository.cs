using System.Linq;
using System.Threading.Tasks;
using VeterinaryClinic.Data.Entities;
using VeterinaryClinic.Enums;
using VeterinaryClinic.Migrations;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Repositories
{
    public interface IAppointmentRespository : IGenericRepository<Appointment>
    {
     
        Task AddItemToAppointmenteAsync(AppointmentDetailsViewModel model, string userName);

        Task<Appointment> ConfirmAppointmentAsync(string userName);

        Task<Appointment> CancelAppointmentAsync(int id);

        Task<Appointment> ConcludeAppointmentAsync(int id);

        Task DeleteDetailTempAsync(int id);

        Task EditAppointmentDetailTempAsync(AppointmentDetailsViewModel model, string username);

        Task EditAppointmentAsync(AppointmentViewModel model, string username);

        Task<Appointment> GetAppointmentByIdAsync(int id);

        Task<IQueryable<Appointment>> GetAppointmentAsync(string userName);

        Task<AppointmentDetailTemp> GetAppointmentDetailTempAsync(int id);  

        Task<IQueryable<AppointmentDetailTemp>> GetDetailsTempsAsync(string userName);

        public IQueryable GetNotificationsAsync();

        public IQueryable GetAppointmentsWithUser();

        public IQueryable GetAppointmentsDetails();

        public IQueryable GetAppointmentsDetailsTemp();

        Task ModifyAppointmentDetailAsync(int id);

        Task SendAppointmentNotification(Appointment appointment, string username, NotificationTypes notificationTypes);
    }
}
