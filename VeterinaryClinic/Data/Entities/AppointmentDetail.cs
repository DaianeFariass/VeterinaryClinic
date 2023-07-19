using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;

namespace VeterinaryClinic.Data.Entities
{
    public class AppointmentDetail : IEntity
    {
        public int Id { get; set; }

        [Required]
        public Pet Pet { get; set; }

        [Required]
        public Vet Vet { get; set; }

        [Required]
        [Display(Name = "Appointment date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime Date { get; set; }

        [Display(Name = "Appointment Time")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime Time { get; set; }

    }
}
