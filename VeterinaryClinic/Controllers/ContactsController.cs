﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using System.Linq;
using VeterinaryClinic.Data.Entities;
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

        public ContactsController(IPetRepository petRepository,
            IVetRepository vetRepository,
            IMailHelper mailHelper) 
        {
            _petRepository = petRepository;
            _vetRepository = vetRepository;
            _mailHelper = mailHelper;
        } 
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
        public IActionResult SendMail(ContactViewModel model)
        {
            Response response = _mailHelper.SendEmail(model.Email, model.Subject, model.Message);
           
            if (response.IsSuccess)
            {
               
                ViewBag.Message = "The email was sent successfully!!!";
               
            }

            return RedirectToAction("Index");


        }


    }
}
