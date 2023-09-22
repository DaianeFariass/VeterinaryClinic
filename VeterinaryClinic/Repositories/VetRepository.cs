using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VeterinaryClinic.Data;
using VeterinaryClinic.Data.Entities;
using VeterinaryClinic.Helpers;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Repositories
{
    public class VetRepository : GenericRepository<Vet>, IVetRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IBlobHelper _blobHelper;

        public VetRepository(DataContext context,
            IUserHelper userHelper,
            IConverterHelper converterHelper,
            IBlobHelper blobHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
            _blobHelper = blobHelper;
        }
        /// <summary>
        /// Método que retorna os vets com o user.
        /// </summary>
        /// <returns>Vets</returns>
        public IQueryable GetAllWithUsers()
        {
            return _context.Vets.Include(v => v.User);
        }
        /// <summary>
        /// Método que preenche a combo vets.
        /// </summary>
        /// <returns>Vets</returns>
        public IEnumerable<SelectListItem> GetComboVets()
        {
            var list = _context.Vets.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString(),

            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select the Vet...)",
                Value = "0"
            });

            return list;
        }
        /// <summary>
        /// Método que preenche a combo specialities.
        /// </summary>
        /// <returns>Specialities</returns>
        public IEnumerable<SelectListItem> GetComboSpecialities()
        {
            var model = new VetViewModel
            {
                Specialities = new List<SelectListItem>
                {
                    new SelectListItem{Text = "Select the Speciality...",Value = "" },
                    new SelectListItem{Text = "Anesthesia and analgesia", Value = "Anesthesia and analgesia"},
                    new SelectListItem{Text = "Cardiology", Value = "Cardiology"},
                    new SelectListItem{Text = "Dermatology", Value = "Dermatology"},
                    new SelectListItem{Text = "Neurology", Value = "Neurology"},
                    new SelectListItem{Text = "Ophthalmology", Value = "Ophthalmology"},
                    new SelectListItem{Text = "Pathology", Value = "Pathology"},
                    new SelectListItem{Text = "Surgery", Value = "Surgery"},

                },
            };
            return model.Specialities;
        }
        /// <summary>
        /// Método para adicionar um vet.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userName"></param>
        /// <returns>Vet</returns>
        public async Task AddSpecialityToVetAsync(VetViewModel model, string userName)
        {
            Guid imageId = Guid.Empty;

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "vets");

            }
            var vet = _converterHelper.ToVet(model, imageId, true);
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return;

            }
            var vetIndex = await _context.Vets
                .Where(v => v.User == user)
                .FirstOrDefaultAsync();

            vetIndex = new VetViewModel
            {
                ImageId = imageId,
                Id = model.Id,
                Name = model.Name,
                Address = model.Address,
                Phone = model.Phone,
                Email = model.Email,
                Speciality = model.SpecialityId,
                SpecialityId = model.SpecialityId,
                User = user,

            };
            _context.Vets.Add(vetIndex);

            await _context.SaveChangesAsync();
        }
    }
}
