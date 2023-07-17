﻿using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;

namespace VeterinaryClinic.Data.Entities
{
    public class Vet : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters lenght.")]
        [Display(Name = "Vet Name")]
        public string Name { get; set; }

        public string Address { get; set; }
   
        public string Phone { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Room { get; set; }

        public User User { get; set; }

        public string ImageFullPath => ImageId == Guid.Empty
                ? $" https://veterinaryclinic.azurewebsites.net/images//imagemindisponivel.png"
                : $" https://veterinaryclinicsystem.blob.core.windows.net/vets/{ImageId}";

    }
}
