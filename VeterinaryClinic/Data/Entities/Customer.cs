using System;
using System.ComponentModel.DataAnnotations;

namespace VeterinaryClinic.Data.Entities
{
    public class Customer : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Image")]
        public Guid? ImageId { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters lenght.")]
        public string Name { get; set; }

        [Required]
        [MaxLength(9, ErrorMessage = "The field {0} can contain {1} characters lenght.")]
        public string Document { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Email { get; set; }

        public User User { get; set; }

        public string ImageFullPath => ImageId == Guid.Empty
             ? $" https://veterinaryclinic.azurewebsites.net/images//imagemindisponivel.png"
             : $" https://veterinaryclinicsystem.blob.core.windows.net/customers/{ImageId}";

    }
}
