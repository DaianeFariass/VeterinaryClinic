using System.Linq;
using VeterinaryClinic.Data.Entities;

namespace VeterinaryClinic.Repositories
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        public IQueryable GetAllWithUsers();
    }
}
