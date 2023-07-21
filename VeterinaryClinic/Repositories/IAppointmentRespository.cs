using System.Linq;
using System.Threading.Tasks;
using VeterinaryClinic.Data.Entities;
using VeterinaryClinic.Migrations;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Repositories
{
    public interface IAppointmentRespository : IGenericRepository<Appointment>
    {
        Task<IQueryable<Appointment>> GetApointmentAsync(string userName);

        Task<IQueryable<AppointmentDetailTemp>> GetDetailsTempsAsync(string userName);

        Task AddItemToAppointmenteAsync(AppointmentViewModel model, string userName);

        Task ModifyAppointmentDetailAsync(int id);

        Task DeleteDetailTempAsync(int id);

        Task<bool> ConfirmAppointmentAsync(string userName);

        public Task DeleteAppointment(int id);

        Task<AppointmentDetailTemp> GetAppointmentDetailTempAsync(int id);

        Task EditAppointmentDetailTempAsync(EditAppointmentDetailTempViewModel model);
    }
}
