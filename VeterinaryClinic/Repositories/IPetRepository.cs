using System.Linq;
using VeterinaryClinic.Data.Entities;

namespace VeterinaryClinic.Repositories
{
    public interface IPetRepository : IGenericRepository<Pet>
    {
        public IQueryable GetAllWithCustomers();
    }
}
