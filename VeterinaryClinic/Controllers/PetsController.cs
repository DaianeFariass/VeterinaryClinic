using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Vereyon.Web;
using VeterinaryClinic.Data;
using VeterinaryClinic.Helpers;
using VeterinaryClinic.Models;
using VeterinaryClinic.Repositories;

namespace VeterinaryClinic.Controllers
{
    [Authorize(Roles = "Vet")]
    public class PetsController : Controller
    {
        private readonly DataContext _context;
        private readonly IPetRepository _petRepository;
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IFlashMessage _flashMessage;

        public PetsController(IPetRepository petRepository,
            IUserHelper userHelper,
            IConverterHelper converterHelper,
            IBlobHelper blobHelper,
            IFlashMessage flashMessage,
            DataContext context)
        {
            _petRepository = petRepository;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
            _blobHelper = blobHelper;
            _flashMessage = flashMessage;
            _context = context;
        }

        // GET: Pets
        public IActionResult Index()
        {
            return View(_petRepository.GetCustomerName());
        }

        // GET: Pets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            var pet = await _petRepository.GetByIdAsync(id.Value);
            if (pet == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            return View(pet);
        }

        // GET: Pets/Create

        public IActionResult Create()
        {
            var model = new PetViewModel
            {
                ImageId = Guid.NewGuid(),
                DateOfBirth = DateTime.Now.Date,
                Types = _petRepository.GetComboTypes(),
                Customers = _petRepository.GetComboCustomers(),
            };
            return View(model);
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
                if (model.DateOfBirth > DateTime.Now.Date)
                {
                    _flashMessage.Warning("Date Invalid!");
                    model = new PetViewModel
                    {
                        ImageId = Guid.NewGuid(),
                        DateOfBirth = DateTime.Now.Date,
                        Types = _petRepository.GetComboTypes(),
                        Customers = _petRepository.GetComboCustomers(),
                    };
                    return View(model);

                }
                else
                {
                    await _petRepository.AddCustomerToPetAsync(model, this.User.Identity.Name);
                    return RedirectToAction(nameof(Index));

                }

            }
            return View(model);
        }

        // GET: Pets/Edit/5

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }
            var pet = await _petRepository.GetByIdAsync(id.Value);
            if (pet == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }
            //var model = _converterHelper.ToPetViewModel(pet);

            var model = new PetViewModel
            {
                Id = pet.Id,
                ImageId = pet.ImageId,
                Name= pet.Name,
                DateOfBirth = DateTime.Now.Date,
                Types = _petRepository.GetComboTypes(),
                Gender = pet.Gender,
                Customers = _petRepository.GetComboCustomers(),
            };
            
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
                    if (model.DateOfBirth > DateTime.Now.Date)
                    {
                        _flashMessage.Warning("Date Invalid!");
                        model = new PetViewModel
                        {
                            ImageId = Guid.NewGuid(),
                            DateOfBirth = DateTime.Now.Date,
                            Types = _petRepository.GetComboTypes(),
                            Customers = _petRepository.GetComboCustomers(),
                        };
                        return View(model);

                    }
                    else
                    {
                        await _petRepository.EditCustomerToPetAsync(model, this.User.Identity.Name);

                    }       
              
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _petRepository.ExistAsync(model.Id))
                    {
                        return new NotFoundViewResult("PetNotFound");
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
                return new NotFoundViewResult("PetNotFound");
            }

            var pet = await _petRepository.GetByIdAsync(id.Value);
            if (pet == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            return View(pet);
        }

        // POST: Pets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pet = await _petRepository.GetByIdAsync(id);
            try
            {
                await _petRepository.DeleteAsync(pet);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {

                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"{pet.Name} probably in been used!!!";
                    ViewBag.ErrorMessage = $"{pet.Name} can not be deleted because there are appointments with this Pet!!!</br></br>" +
                        $"First delete all the appointments with this Pet" +
                        $" And Please try again delete it";

                }
                return View("Error");
            }

        }
        public IActionResult PetNotFound()
        {
            return View();
        }


    }
}
