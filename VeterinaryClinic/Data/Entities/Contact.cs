using System.ComponentModel.DataAnnotations;

namespace VeterinaryClinic.Data.Entities
{
    public class Contact : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        public string Phone { get; set; }

        [Required]
        public string Message { get; set; }

        public Customer Customer { get; set; }

        public Vet Vet { get; set; }

    }
}
