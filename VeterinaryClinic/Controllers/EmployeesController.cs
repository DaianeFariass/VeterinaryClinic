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
    public class EmployeesController : Controller
    {
        private readonly DataContext _context;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IBlobHelper _blobHelper;

        public EmployeesController(IEmployeeRepository employeeRepository,
            IUserHelper userHelper,
            IConverterHelper converterHelper,
            IBlobHelper blobHelper)
        {
            _employeeRepository = employeeRepository;
            _userHelper = userHelper;       
            _converterHelper= converterHelper;
            _blobHelper= blobHelper;
        }

        // GET: Employees
        public IActionResult Index()
        {
            return View(_employeeRepository.GetAll().OrderBy(e => e.Name));
        }

        // GET: Employees/Details/5
        [Route("detailsemployee")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("EmployeeNotFound");
            }

            var employee = await _employeeRepository.GetByIdAsync(id.Value);
            if (employee == null)
            {
                return new NotFoundViewResult("EmployeeNotFound");
            }

            return View(employee);
        }

        // GET: Employees/Create
        [Route("createemployee")]
        public IActionResult Create()
        {
            var model = new EmployeeViewModel
            {
                Roles = _employeeRepository.GetComboRoles(),
            };
            ViewBag.Roles = model.Roles;
            return View(model);
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("createemployee")]
        public async Task<IActionResult> Create(EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _employeeRepository.AddRoleToEmployeeAsync(model, this.User.Identity.Name);
             
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Employees/Edit/5
        [Route("editemployee")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("EmployeeNotFound");
            }

            var employee = await _employeeRepository.GetByIdAsync(id.Value);
            if (employee == null)
            {
                return new NotFoundViewResult("EmployeeNotFound");
            }
            var model = new EmployeeViewModel
            {
                Roles = _employeeRepository.GetComboRoles(),

            };
            ViewBag.Roles = model.Roles;
            return View(model);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("editemployee")]
        public async Task<IActionResult> Edit(EmployeeViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = Guid.Empty;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {

                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "employees");

                    }
                    var employee = _converterHelper.ToEmployee(model, imageId, false);
                    employee.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                    employee.Role = model.RoleId;
                    await _employeeRepository.UpdateAsync(employee);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _employeeRepository.ExistAsync(model.Id))
                    {
                        return new NotFoundViewResult("EmployeeNotFound");
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

        // GET: Employees/Delete/5
        [Route("deleteemployee")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("EmployeeNotFound");
            }

            var employee = await _employeeRepository.GetByIdAsync(id.Value);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("deleteemployee")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            try
            {
                await _employeeRepository.DeleteAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {

                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"{employee.Name} probably in been used!!!";
                    ViewBag.ErrorMessage = $"{employee.Name} can not be deleted!!!</br></br>"; 
                      

                }
               
            }
           
            return View("Error");
        }
        [Route("employeenotfound")]
        public IActionResult EmployeeNotFound()
        {
            return View();
        }

    }
}
