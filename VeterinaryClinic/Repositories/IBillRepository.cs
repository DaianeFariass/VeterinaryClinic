using System.Linq;
using VeterinaryClinic.Data.Entities;

namespace VeterinaryClinic.Repositories
{
    public interface IBillRepository : IGenericRepository<Bill>
    {
        public IQueryable GetBills();
    }
}
