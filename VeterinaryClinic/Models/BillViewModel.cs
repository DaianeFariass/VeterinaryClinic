using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VeterinaryClinic.Data.Entities;

namespace VeterinaryClinic.Models
{
    public class BillViewModel : Bill
    {
        [Display(Name = "Appointments")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select the Customer!")]
        public int AppointmentId { get; set; }
        public IEnumerable<SelectListItem> Appointments { get; set; }

    }
}
