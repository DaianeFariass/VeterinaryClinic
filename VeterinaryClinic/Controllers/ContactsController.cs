using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vereyon.Web;
using VeterinaryClinic.Data;
using VeterinaryClinic.Helpers;
using VeterinaryClinic.Models;
using VeterinaryClinic.Repositories;

namespace VeterinaryClinic.Controllers
{
    public class ContactsController : Controller
    {
        private readonly IPetRepository _petRepository;
        private readonly IVetRepository _vetRepository;
        private readonly IMailHelper _mailHelper;
        private readonly IFlashMessage _flashMessage;
        private readonly DataContext _context;

        public ContactsController(IPetRepository petRepository,
            IVetRepository vetRepository,
            IMailHelper mailHelper,
            IFlashMessage flashMessage,
            DataContext context)
        {
            _petRepository = petRepository;
            _vetRepository = vetRepository;
            _mailHelper = mailHelper;
            _flashMessage = flashMessage;
            _context = context;
        }
        [Authorize(Roles = "Director, Admin, Vet, Assistant, Receptionist")]
        public IActionResult Index()
        {
            var model = new ContactViewModel
            {
                Customers = _petRepository.GetComboCustomersEmail(),
                Vets = _vetRepository.GetComboVets(),

            };

            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Director, Admin, Vet, Assistant, Receptionist")]
        public IActionResult SendMail(ContactViewModel model)
        {
            Response response = _mailHelper.SendEmail(model.Email, model.Subject, model.Message);
            _context.Contacts.Add(model);
            _context.SaveChangesAsync();
            if (response.IsSuccess)
            {

                _flashMessage.Confirmation("The email was sent successfully!!!");
                return RedirectToAction("Index");

            }

            return RedirectToAction("Index");


        }



    }
}
