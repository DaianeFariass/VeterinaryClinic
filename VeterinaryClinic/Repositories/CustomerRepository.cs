using Microsoft.EntityFrameworkCore;
using System.Linq;
using VeterinaryClinic.Data;
using VeterinaryClinic.Data.Entities;

namespace VeterinaryClinic.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly DataContext _context;

        public CustomerRepository(DataContext context) : base(context) 
        {
            _context = context;
        }
        public IQueryable GetAllWithUsers()
        {
            return _context.Customers.Include(c => c.User);
        }

    }
}
