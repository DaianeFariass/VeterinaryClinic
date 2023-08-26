using System.Linq;
using System.Threading.Tasks;
using VeterinaryClinic.Data.Entities;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Repositories
{
    public interface IPetReportRepository : IGenericRepository<PetReport>
    {
        public IQueryable GetPetReport();

        public Task AddItemToPetReportAsync(PetReportViewModel model, string userName);

        public Task EditPetReportAsync(PetReportViewModel model, string username);

        public Task DeletePetReportAsync(int id);
    }
}
