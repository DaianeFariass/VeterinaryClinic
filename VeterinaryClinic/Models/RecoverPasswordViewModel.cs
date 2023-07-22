using System.ComponentModel.DataAnnotations;

namespace VeterinaryClinic.Models
{
    public class RecoverPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
