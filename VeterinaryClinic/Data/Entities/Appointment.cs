﻿using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VeterinaryClinic.Data.Entities
{
    public class Appointment : IEntity
    {
        public int Id { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        [Display(Name = "Appointment date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Time")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime Time { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public IEnumerable<AppointmentDetail> Items { get; set; }

        public int Lines => Items == null ? 0 : Items.Count();
   

    }
}