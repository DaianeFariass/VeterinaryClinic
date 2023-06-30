using Microsoft.EntityFrameworkCore;
using System.Linq;
using VeterinaryClinic.Data;
using VeterinaryClinic.Data.Entities;

namespace VeterinaryClinic.Repositories
{
    public class PetRepository : GenericRepository<Pet>, IPetRepository
    {
        private readonly DataContext _context;

        public PetRepository(DataContext context) : base(context) 
        {
            _context = context;
        }

        public IQueryable GetAllWithCustomers()
        {
            return _context.Pets.Include(p => p.Customer);
        }
    }
}
