using System.Linq;
using System.Threading.Tasks;
using VeterinaryClinic.Data.Entities;

namespace VeterinaryClinic.Repositories
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task DeleteNotificationAsync(int id);

        IQueryable GetNotificationsAsync();


    }
}
