using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using VeterinaryClinic.Data.Entities;

namespace VeterinaryClinic.Repositories
{
    public interface IVetRepository : IGenericRepository<Vet>
    {
        public IQueryable GetAllWithUsers();

        IEnumerable<SelectListItem> GetComboVets();
    }
}
