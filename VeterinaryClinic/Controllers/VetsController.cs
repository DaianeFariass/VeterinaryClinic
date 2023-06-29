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
using VeterinaryClinic.Repositories;

namespace VeterinaryClinic.Controllers
{
    public class VetsController : Controller
    {
       
        private readonly IVetRepository _vetRepository;
        private readonly IUserHelper _userHelper;

        public VetsController(IVetRepository vetRepository,
            IUserHelper userHelper)
        {
          _vetRepository = vetRepository;
          _userHelper = userHelper;
        }

        // GET: Vets
        public IActionResult Index()
        {
            return View(_vetRepository.GetAll().OrderBy(v => v.Name));
        }

        // GET: Vets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vet = await _vetRepository.GetByIdAsync(id.Value);
            if (vet == null)
            {
                return NotFound();
            }

            return View(vet);
        }

        // GET: Vets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vet vet)
        {
            var email = Request.Form["Email"].ToString();
            email = vet.Name.Replace(" ", "_") + "@cinel.com";
            var password = Request.Form["Password"].ToString();
            password = "123456";

            if (ModelState.IsValid)
            {
                vet.User = await _userHelper.GetUserByEmailAsync("daiane.farias@cinel.pt");
                await _vetRepository.CreateAsync(vet);

                return RedirectToAction(nameof(Index));
            }
            return View(vet);
        }

        // GET: Vets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vet = await _vetRepository.GetByIdAsync(id.Value);
            if (vet == null)
            {
                return NotFound();
            }
            return View(vet);
        }

        // POST: Vets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Vet vet)
        {
            if (id != vet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    vet.User = await _userHelper.GetUserByEmailAsync("daiane.farias@cinel.pt");
                    await _vetRepository.UpdateAsync(vet);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _vetRepository.ExistAsync(vet.Id))
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
            return View(vet);
        }

        // GET: Vets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vet = await _vetRepository?.GetByIdAsync(id.Value);
            if (vet == null)
            {
                return NotFound();
            }

            return View(vet);
        }

        // POST: Vets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vet = await _vetRepository.GetByIdAsync(id);
            await _vetRepository.DeleteAsync(vet);
            return RedirectToAction(nameof(Index));
        }
    }
}
