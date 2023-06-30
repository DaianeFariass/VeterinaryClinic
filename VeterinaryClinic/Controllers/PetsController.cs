using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VeterinaryClinic.Data;
using VeterinaryClinic.Data.Entities;
using VeterinaryClinic.Helpers;
using VeterinaryClinic.Models;
using VeterinaryClinic.Repositories;

namespace VeterinaryClinic.Controllers
{
    public class PetsController : Controller
    {
        private readonly DataContext _context;
        private readonly IPetRepository _petRepository;
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IBlobHelper _blobHelper;
        public PetsController(IPetRepository petRepository,
            IUserHelper userHelper,
            IConverterHelper converterHelper,
            IBlobHelper blobHelper,
            DataContext context)
        {
            _petRepository = petRepository;
            _userHelper= userHelper;
            _converterHelper = converterHelper;
            _blobHelper = blobHelper;
            _context = context;
        }

        // GET: Pets
        public IActionResult Index()
        {
            return View(_petRepository.GetAll().OrderBy(p => p.Name));
        }

        // GET: Pets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _petRepository.GetByIdAsync(id.Value);
            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }

        // GET: Pets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PetViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "pets");

                }
                var pet = _converterHelper.ToPet(model, imageId, true);
                pet.Customer = _context.Customers.FirstOrDefault();
                await _petRepository.CreateAsync(pet);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Pets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _petRepository.GetByIdAsync(id.Value);

            if (pet == null)
            {
                return NotFound();
            }
            var model = _converterHelper.ToPetViewModel(pet);
            return View(model);
        }

        // POST: Pets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PetViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = Guid.Empty;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {

                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "pets");

                    }
                    var pet = _converterHelper.ToPet(model, imageId,false);
                    pet.Customer = _context.Customers.FirstOrDefault();
                    await _petRepository.UpdateAsync(pet);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _petRepository.ExistAsync(model.Id))
                    {
                        return NotFound();
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

        // GET: Pets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _petRepository.GetByIdAsync(id.Value);
            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }

        // POST: Pets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pet = await _petRepository.GetByIdAsync(id);
            await _petRepository.DeleteAsync(pet);
            return RedirectToAction(nameof(Index));
        }

       
    }
}
