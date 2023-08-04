using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VeterinaryClinic.Data.Entities;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Repositories
{
    public interface IPetRepository : IGenericRepository<Pet>
    {
        public IQueryable GetAllWithCustomers();

        IEnumerable<SelectListItem> GetComboCustomers();

        IEnumerable<SelectListItem> GetComboPets();

        IEnumerable<SelectListItem> GetComboTypes();

        public IQueryable GetCustomerName();

        public Task AddCustomerToPetAsync(PetViewModel model, string userName);

        public Task EditCustomerToPetAsync(PetViewModel model, string userName);





    }
}
