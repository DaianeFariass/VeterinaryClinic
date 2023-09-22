using System;
using System.ComponentModel.DataAnnotations;
using VeterinaryClinic.Enums;

namespace VeterinaryClinic.Data.Entities
{
    public class Appointment : IEntity
    {
        public int Id { get; set; }


        [Display(Name = "User Name")]
        public User User { get; set; }


        [Display(Name = "Pet Name")]
        public Pet Pet { get; set; }


        [Display(Name = "Vet Name")]
        public Vet Vet { get; set; }

        [Required]
        [Display(Name = "Appointment Confirmed date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Appointment Confirmed Time")]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime Time { get; set; }


        [Display(Name = "Appointment Status")]
        public StatusAppointment Status { get; set; }


    }
}
