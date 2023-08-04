using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using VeterinaryClinic.Data.Entities;

namespace VeterinaryClinic.Models
{
    public class RoomViewModel : Room
    {

        [Display(Name = "Vet Name")]
        public int? VetId { get; set; }
        public IEnumerable<SelectListItem> Vets { get; set; }
            
        [Display(Name = "Room Type")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select the Type!")]
        public int TypeId { get; set; }
        public IEnumerable<SelectListItem> Types { get; set; }

        public bool IsEditing { get; set; }

    }
}
