using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using VeterinaryClinic.Data;
using VeterinaryClinic.Data.Entities;
using VeterinaryClinic.Helpers;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Repositories
{
    public class PetReportRepository : GenericRepository<PetReport>, IPetReportRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;

        public PetReportRepository(DataContext context,
            IUserHelper userHelper,
            IConverterHelper converterHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
        }
        /// <summary>
        /// Método para criar um pet report.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userName"></param>
        /// <returns>Pet Report</returns>
        public async Task AddItemToPetReportAsync(PetReportViewModel model, string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return;

            }
            var pet = await _context.Pets.FindAsync(model.PetId);
            if (pet == null)
            {
                return;
            }
            var vet = await _context.Vets.FindAsync(model.VetId);
            if (vet == null)
            {
                return;

            }
            var petReport = new PetReportViewModel
            {
                Pet = pet,
                Vet = vet,
                TestName = model.TestName,
                Diagnose = model.Diagnose,
                MedicineName = model.MedicineName,

            };
            _context.PetReports.Add(petReport);

            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// Método para editar o pet report.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="username"></param>
        /// <returns>Pet Report</returns>
        public async Task EditPetReportAsync(PetReportViewModel model, string username)
        {
            _converterHelper.ToPetReport(model, false);

            var user = await _userHelper.GetUserByEmailAsync(username);
            if (user == null)
            {
                return;

            }
            var pet = await _context.Pets.FindAsync(model.PetId);
            if (pet == null)
            {
                return;
            }
            var vet = await _context.Vets.FindAsync(model.VetId);
            if (vet == null)
            {
                return;
            }
            var petReport = new PetReportViewModel
            {
                Id = model.Id,
                Vet = vet,
                VetId = model.VetId,
                Pet = pet,
                PetId = model.PetId,
                TestName = model.TestName,
                Diagnose = model.Diagnose,
                MedicineName = model.MedicineName,


            };
            _context.PetReports.Update(petReport);
            await _context.SaveChangesAsync();

        }
        /// <summary>
        /// Método para Deletar um pet report.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Deleta Pet Report</returns>
        public async Task DeletePetReportAsync(int id)
        {
            var petReport = await _context.PetReports.FindAsync(id);
            if (petReport == null)
            {
                return;

            }
            _context.PetReports.Remove(petReport);
            await _context.SaveChangesAsync();

        }
        /// <summary>
        /// Método que retorna todos o pet reports.
        /// </summary>
        /// <returns>Pet Reports</returns>
        public IQueryable GetPetReport()
        {
            return _context.PetReports
               .Include(p => p.Pet)
               .Include(v => v.Vet);
        }
    }
}
