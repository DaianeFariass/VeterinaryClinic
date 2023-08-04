using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VeterinaryClinic.Data.Entities;

namespace VeterinaryClinic.Models
{
    public class AppointmentViewModel 
    {
        
        [Display(Name="Pet Name")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select your Pet!")]
        public int PetId { get; set; }

        [Display(Name = "Vet Name")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select the Vet!")]
        public int VetId { get; set; }

        [Required]
        [Display(Name = "Appointment date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Appointment Time")]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime Time { get; set; }

        public IEnumerable<SelectListItem> Pets { get; set; }

        public IEnumerable<SelectListItem> Vets { get; set; }

        public IEnumerable<SelectListItem> Times { get; set; }


    }
}
