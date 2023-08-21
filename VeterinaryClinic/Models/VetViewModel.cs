using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using VeterinaryClinic.Data.Entities;

namespace VeterinaryClinic.Models
{
    public class VetViewModel : Vet
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }

        [Display(Name = "Speciality")]
        public string SpecialityId { get; set; }
        public IEnumerable<SelectListItem> Specialities { get; set; }
    }
}
