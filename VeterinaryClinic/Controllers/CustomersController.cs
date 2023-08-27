using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using VeterinaryClinic.Helpers;
using VeterinaryClinic.Models;
using VeterinaryClinic.Repositories;

namespace VeterinaryClinic.Controllers
{
    [Authorize(Roles = "Vet")]
    public class CustomersController : Controller
    {

        private readonly ICustomerRepository _customerRepository;
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IBlobHelper _blobHelper;

        public CustomersController(ICustomerRepository customerRepository,
            IUserHelper userHelper,
            IConverterHelper converterHelper,
            IBlobHelper blobHelper)
        {
            _customerRepository = customerRepository;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
            _blobHelper = blobHelper;
        }

        // GET: Customers
        public IActionResult Index()
        {
            return View(_customerRepository.GetAll().OrderBy(c => c.Name));
        }

        // GET: Customers/Details/5
        [Route("detailscustomer")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("CustomerNotFound");
            }

            var customer = await _customerRepository.GetByIdAsync(id.Value);

            if (customer == null)
            {
                return new NotFoundViewResult("CustomerNotFound");
            }

            return View(customer);
        }

        // GET: Customers/Create
        [Route("createcustomer")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("createcustomer")]
        public async Task<IActionResult> Create(CustomerViewModel model)
        {
            var email = Request.Form["Email"].ToString();
            email = model.Name.Replace(" ", "_") + "@cinel.com";
            var password = Request.Form["Password"].ToString();
            password = "123456";

            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "customers");

                }

                var customer = _converterHelper.ToCustomer(model, imageId, true);
                customer.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                await _customerRepository.CreateAsync(customer);
                return RedirectToAction(nameof(Index));
            };
            return View(model);
        }


        // GET: Customers/Edit/5
        [Route("editcustomer")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("CustomerNotFound");
            }

            var customer = await _customerRepository.GetByIdAsync(id.Value);
            if (customer == null)
            {
                return new NotFoundViewResult("CustomerNotFound");
            }
            var model = _converterHelper.ToCustomerViewModel(customer);
            return View(model);
        }


        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("editcustomer")]
        public async Task<IActionResult> Edit(CustomerViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = Guid.Empty;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {

                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "customers");

                    }

                    var customer = _converterHelper.ToCustomer(model, imageId, false);
                    customer.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                    await _customerRepository.UpdateAsync(customer);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _customerRepository.ExistAsync(model.Id))
                    {
                        return new NotFoundViewResult("CustomerNotFound");
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

        // GET: Customers/Delete/5
        [Route("deletecustomer")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("CustomerNotFound");
            }

            var customer = await _customerRepository.GetByIdAsync(id.Value);
            if (customer == null)
            {
                return new NotFoundViewResult("CustomerNotFound");
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("deletecustomer")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            try
            {
                await _customerRepository.DeleteAsync(customer);
                return RedirectToAction(nameof(Index));

            }
            catch (DbUpdateException ex)
            {

                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"{customer.Name} probably in been used!!!";
                    ViewBag.ErrorMessage = $"{customer.Name} can not be deleted because there are appointments with this customer!!!</br></br>" +
                        $"First delete all the appointments with this customer" +
                        $" And Please try again delete it";

                }
                return View("Error");

            }
        }
        public IActionResult CustomerNotFound()
        {
            return View();
        }

    }
}
