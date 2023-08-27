using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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








    }
}
