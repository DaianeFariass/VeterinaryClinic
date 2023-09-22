using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VeterinaryClinic.Data.Entities;

namespace VeterinaryClinic.Models
{
    public class RoomViewModel : Room
    {

        [Display(Name = "Vet Name")]
        public int? VetId { get; set; }
        public IEnumerable<SelectListItem> Vets { get; set; }

        [Display(Name = "Room Type")]
        public string TypeId { get; set; }
        public IEnumerable<SelectListItem> Types { get; set; }


    }
}
