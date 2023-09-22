using System.ComponentModel.DataAnnotations;

namespace VeterinaryClinic.Data.Entities
{
    public class PetReport : IEntity
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Test Name")]
        public string TestName { get; set; }

        [Required]
        public string Diagnose { get; set; }

        [Required]
        public string MedicineName { get; set; }

        public Vet Vet { get; set; }

        public Pet Pet { get; set; }



    }
}
