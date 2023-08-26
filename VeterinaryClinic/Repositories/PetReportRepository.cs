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

        public PetReportRepository(DataContext context,
            IUserHelper userHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
        }

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

        public async Task EditPetReportAsync(PetReportViewModel model, string username)
        {
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
            var petReport = await _context.PetReports.FindAsync(model.Id);

            petReport.Pet = pet;
            petReport.Vet = vet;
            petReport.TestName = model.TestName;
            petReport.Diagnose = model.Diagnose;
            petReport.MedicineName = model.MedicineName;

            _context.PetReports.Update(petReport);

            await _context.SaveChangesAsync();

        }
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

        public IQueryable GetPetReport()
        {
            return _context.PetReports
               .Include(p => p.Pet)
               .Include(v => v.Vet);
        }
    }
}
