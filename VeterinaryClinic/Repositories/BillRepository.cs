using Microsoft.EntityFrameworkCore;
using System.Linq;
using VeterinaryClinic.Data;
using VeterinaryClinic.Data.Entities;

namespace VeterinaryClinic.Repositories
{
    public class BillRepository : GenericRepository<Bill>, IBillRepository
    {
        private readonly DataContext _context;

        public BillRepository(DataContext context) : base(context)
        {
            _context = context;
        } 
        public IQueryable GetBills()
        {
            return _context.Bills
                .Include(b => b.User)
                .Include(b => b.Appointment)
                .ThenInclude(b => b.Vet)
                .Include(b => b.Appointment)
                .ThenInclude(b => b.Pet)
                .ThenInclude(b => b.Customer);

        }

       
    }
}
