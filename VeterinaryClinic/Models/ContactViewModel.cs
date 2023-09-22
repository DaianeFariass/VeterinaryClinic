﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VeterinaryClinic.Data.Entities;

namespace VeterinaryClinic.Models
{
    public class ContactViewModel : Contact
    {
        [Display(Name = "Customer Name")]
        public int? CustomerId { get; set; }
        public IEnumerable<SelectListItem> Customers { get; set; }

        [Display(Name = "Vet Name")]
        public int? VetId { get; set; }
        public IEnumerable<SelectListItem> Vets { get; set; }

    }
}
