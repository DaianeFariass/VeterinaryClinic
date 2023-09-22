using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using VeterinaryClinic.Helpers;
using VeterinaryClinic.Models;
using VeterinaryClinic.Repositories;

namespace VeterinaryClinic.Controllers
{
    [Authorize(Roles = "Admin")]
    public class VetsController : Controller
    {

        private readonly IVetRepository _vetRepository;
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IBlobHelper _blobHelper;

        public VetsController(IVetRepository vetRepository,
            IUserHelper userHelper,
            IConverterHelper converterHelper,
            IBlobHelper blobHelper)
        {
            _vetRepository = vetRepository;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
            _blobHelper = blobHelper;
        }

        // GET: Vets
        public IActionResult Index()
        {
            return View(_vetRepository.GetAll().OrderBy(v => v.Name));
        }

        // GET: Vets/Details/5
        [Route("detailsvet")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("VetNotFound");
            }

            var vet = await _vetRepository.GetByIdAsync(id.Value);
            if (vet == null)
            {
                return new NotFoundViewResult("VetNotFound");
            }

            return View(vet);
        }

        // GET: Vets/Create
        [Route("createvet")]
        public IActionResult Create()
        {
            var model = new VetViewModel
            {
                Specialities = _vetRepository.GetComboSpecialities(),

            };
            ViewBag.Specialities = model.Specialities;
            return View(model);
        }

        // POST: Vets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("createvet")]
        public async Task<IActionResult> Create(VetViewModel model)
        {

            if (ModelState.IsValid)
            {
                await _vetRepository.AddSpecialityToVetAsync(model, this.User.Identity.Name);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Vets/Edit/5
        [Route("editvet")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("VetNotFound");
            }

            var vet = await _vetRepository.GetByIdAsync(id.Value);
            if (vet == null)
            {
                return new NotFoundViewResult("VetNotFound");
            }
            var model = new VetViewModel
            {
                Specialities = _vetRepository.GetComboSpecialities(),

            };
            ViewBag.Specialities = model.Specialities;
            model = _converterHelper.ToVetViewModel(vet);
            return View(model);
        }

        // POST: Vets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("editvet")]
        public async Task<IActionResult> Edit(VetViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = Guid.Empty;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {

                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "vets");

                    }
                    var vet = _converterHelper.ToVet(model, imageId, false);
                    vet.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                    vet.Speciality = model.SpecialityId;
                    await _vetRepository.UpdateAsync(vet);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _vetRepository.ExistAsync(model.Id))
                    {
                        return new NotFoundViewResult("VetNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Vets/Delete/5
        [Route("deletevet")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("VetNotFound");
            }

            var vet = await _vetRepository.GetByIdAsync(id.Value);
            if (vet == null)
            {
                return new NotFoundViewResult("VetNotFound");
            }

            return View(vet);
        }

        // POST: Vets/Delete/5    
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("deletevet")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vet = await _vetRepository.GetByIdAsync(id);
            try
            {
                await _vetRepository.DeleteAsync(vet);
                return RedirectToAction(nameof(Index));

            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"{vet.Name} probably in been used!!!";
                    ViewBag.ErrorMessage = $"{vet.Name} can not be deleted because there are appointments with this Vet!!!</br></br>" +
                        $"First delete all the appointments with this Vet" +
                        $"And Please try again delete it";

                }
                return View("Error");
            }

        }
        [Route("vetnotfound")]
        public IActionResult VetNotFound()
        {
            return View();
        }
    }
}
