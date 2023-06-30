using Microsoft.EntityFrameworkCore;
using System.Linq;
using VeterinaryClinic.Data;
using VeterinaryClinic.Data.Entities;

namespace VeterinaryClinic.Repositories
{
    public class VetRepository : GenericRepository<Vet>, IVetRepository
    {
        private readonly DataContext _context;

        public VetRepository(DataContext context) : base(context) 
        {
            _context = context;
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.Vets.Include(v => v.User);
        }
    }
}
