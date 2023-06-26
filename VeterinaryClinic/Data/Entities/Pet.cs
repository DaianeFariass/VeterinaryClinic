using System;
using System.ComponentModel.DataAnnotations;

namespace VeterinaryClinic.Data.Entities
{
    public class Pet
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters lenght.")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Birth Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime DateOfBirth { get; set; }

        public string Type { get; set; }

        public string Gender { get; set; }  

        public Customer Customer { get; set; }

    }
}
