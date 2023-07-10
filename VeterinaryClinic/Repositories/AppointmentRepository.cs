using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using VeterinaryClinic.Data;
using VeterinaryClinic.Data.Entities;
using VeterinaryClinic.Helpers;

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

        public async Task<IQueryable<Appointment>> GetApointmentAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null) 
            { 
                return null;
            
            }
            if (await _userHelper.IsUserInRoleAsync(user, "Vet"))
            {
                return _context.Appointments
                    .Include(a => a.Items)
                    .ThenInclude(p => p.Pet)
                    .OrderByDescending(a => a.Date);
            }
            return _context.Appointments
                .Include(a => a.Items)
                .ThenInclude(p => p.Pet)
                .Where(a => a.User == user)
                .OrderByDescending(a => a.Date);

        }
    }
}
