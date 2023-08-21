using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VeterinaryClinic.Data.Entities;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Repositories
{
    public interface IVetRepository : IGenericRepository<Vet>
    {
        public IQueryable GetAllWithUsers();

        IEnumerable<SelectListItem> GetComboVets();

        IEnumerable<SelectListItem> GetComboSpecialities();

        public Task AddSpecialityToVetAsync(VetViewModel model, string userName);
    }
}
