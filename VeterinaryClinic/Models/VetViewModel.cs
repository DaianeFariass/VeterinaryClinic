using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using VeterinaryClinic.Data.Entities;

namespace VeterinaryClinic.Models
{
    public class VetViewModel : Vet
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
    }
}
