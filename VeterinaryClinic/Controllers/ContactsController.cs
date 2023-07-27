using Microsoft.AspNetCore.Mvc;

namespace VeterinaryClinic.Controllers
{
    public class ContactsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
