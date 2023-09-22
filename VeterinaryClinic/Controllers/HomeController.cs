using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Vereyon.Web;
using VeterinaryClinic.Data;
using VeterinaryClinic.Helpers;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMailHelper _mailHelper;
        private readonly IFlashMessage _flashMessage;
        private readonly DataContext _context;

        public HomeController(ILogger<HomeController> logger,
            IMailHelper mailHelper,
            IFlashMessage flashMessage,
            DataContext context)
        {
            _logger = logger;
            _mailHelper = mailHelper;
            _flashMessage = flashMessage;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("about")]
        public IActionResult About()
        {
            return View();
        }

        [Route("services")]
        public IActionResult Services()
        {
            return View();
        }
        [Route("contact")]
        public IActionResult Contact()
        {

            return View();

        }
        [Route("contact")]
        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            Response response = _mailHelper.SendEmail(model.Email, model.Subject, model.Message);
            _context.Contacts.Add(model);
            _context.SaveChangesAsync();
            if (response.IsSuccess)
            {

                _flashMessage.Confirmation("The email was sent successfully!!!");


            }

            return View();

        }
        [HttpPost]
        public IActionResult SendMailHome(ContactViewModel model)
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
