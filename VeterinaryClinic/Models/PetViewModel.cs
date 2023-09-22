using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VeterinaryClinic.Data.Entities;

namespace VeterinaryClinic.Models
{
    public class PetViewModel : Pet
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }

        [Display(Name = "Customer Name")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select the Customer!")]
        public int CustomerId { get; set; }
        public IEnumerable<SelectListItem> Customers { get; set; }

        [Display(Name = "Type")]
        public string TypeId { get; set; }
        public IEnumerable<SelectListItem> Types { get; set; }

        public string CustomerWithImage
        {
            get
            {
                return ImageId + " " + Name;
            }
        }
    }

}
