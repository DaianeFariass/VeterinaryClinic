using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using VeterinaryClinic.Data;
using VeterinaryClinic.Data.Entities;

namespace VeterinaryClinic.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        private readonly DataContext _context;
        public NotificationRepository(DataContext context) : base(context)
        {
            _context = context;
        }
        /// <summary>
        /// Método para deletar as notificações.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Delete Notification</returns>
        public async Task DeleteNotificationAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
            {
                return;

            }
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();

        }
        /// <summary>
        /// Método que retorna todas as notificações.
        /// </summary>
        /// <returns>Notificações</returns>
        public IQueryable GetNotificationsAsync()
        {

            return _context.Notifications
                .Include(n => n.Appointment)
                .ThenInclude(n => n.Pet)
                .ThenInclude(n => n.Customer)
                .Include(n => n.Appointment)
                .ThenInclude(n => n.Vet)
                .Include(n => n.Appointment)
                .ThenInclude(n => n.User);


        }


    }
}
