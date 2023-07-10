using System.Linq;
using System.Threading.Tasks;
using VeterinaryClinic.Data.Entities;

namespace VeterinaryClinic.Repositories
{
    public interface IAppointmentRespository : IGenericRepository<Appointment>
    {
        Task<IQueryable<Appointment>> GetApointmentAsync(string userName);
    }
}
