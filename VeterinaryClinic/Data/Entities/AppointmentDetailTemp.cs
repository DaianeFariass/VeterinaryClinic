using System;
using System.ComponentModel.DataAnnotations;

namespace VeterinaryClinic.Data.Entities
{
    public class AppointmentDetailTemp : IEntity
    {
        public int Id { get; set; }


        public User User { get; set; }


        public Pet Pet { get; set; }


        public Vet Vet { get; set; }

        [Required]
        [Display(Name = "Appointment date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Appointment Time")]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime Time { get; set; }


    }
}
