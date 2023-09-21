using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using System.Linq;
using System.Threading.Tasks;
using VeterinaryClinic.Data;
using VeterinaryClinic.Data.Entities;
using VeterinaryClinic.Helpers;
using VeterinaryClinic.Models;
using VeterinaryClinic.Repositories;

namespace VeterinaryClinic.Controllers
{
    public class BillsController : Controller
    {
        private readonly IBillRepository _billRepository;
        private readonly IConverterHelper _converterHelper;
        private readonly DataContext _context;

        public BillsController(IBillRepository billRepository,
            IConverterHelper converterHelper,
            DataContext context)
        {
            _billRepository = billRepository;
            _converterHelper = converterHelper;
            _context = context;
        }

        // GET: Bills

        public IActionResult Index()
        {
            var model = _billRepository.GetBills();
            return View(model);
        }

        // GET: Bills/Edit/5
        [Route("editbill")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bill = await _billRepository.GetByIdAsync(id.Value);
            if (bill == null)
            {
                return new NotFoundViewResult("BillNotFound");
            }
            var model = _converterHelper.ToBillViewModel(bill);
            return View(model);
        }

        // POST: Bills/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("editbill")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Cost")] Bill bill)
        {
            if (id != bill.Id)
            {
                return new NotFoundViewResult("BillNotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bill);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillExists(bill.Id))
                    {
                        return new NotFoundViewResult("BillNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bill);
        }
        private bool BillExists(int id)
        {
            return _context.Bills.Any(e => e.Id == id);
        }
        [Route("billnotfound")]
        public IActionResult BillNotFound()
        {
            return View();
        }
        [Route("printbill")]
        public IActionResult PrintBill(int id)
        {
            var model = _context.Bills
                 .Include(b => b.Appointment)
                 .ThenInclude(b => b.Pet.Customer)
                 .Include(b => b.User)
                 .Where(b => b.Id == id)
                 .Select(b => new BillViewModel()
                 {
                     Id = b.Id,
                     Appointment = b.Appointment,
                     User = b.User,


                 }).FirstOrDefault();
            return new ViewAsPdf("PrintBill", model)
            {
                FileName = $"Bill {model.Id}.pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageSize = Rotativa.AspNetCore.Options.Size.A4,


            };
        }
    }
}
