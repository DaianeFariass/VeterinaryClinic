using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VeterinaryClinic.Data.Entities
{
    public class Pet : IEntity
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters lenght.")]
        [Display(Name = "Pet Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Birth Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime DateOfBirth { get; set; }

       
        public string Type { get; set; }

      
        public string Gender { get; set; }

      
        public Customer Customer { get; set; }


        public string ImageFullPath => ImageId == Guid.Empty
              ? $" https://provetclinic.azurewebsites.net/images/imagemindisponivel.png"
              : $" https://provetclinic.blob.core.windows.net/pets/{ImageId}";

    }
}
