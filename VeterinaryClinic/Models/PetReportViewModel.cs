using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VeterinaryClinic.Data.Entities;

namespace VeterinaryClinic.Models
{
    public class PetReportViewModel : PetReport
    {
        [Display(Name = "Pet Name")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select your Pet!")]
        public int PetId { get; set; }

        [Display(Name = "Vet Name")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select the Vet!")]
        public int VetId { get; set; }

        public IEnumerable<SelectListItem> Pets { get; set; }

        public IEnumerable<SelectListItem> Vets { get; set; }

    }
}
