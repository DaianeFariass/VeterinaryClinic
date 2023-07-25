using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace VeterinaryClinic.Data.Entities
{
    public class Room : IEntity
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Room")]
        public string RoomNumber { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Status { get; set; }

        public Vet Vet { get; set; }
    }
}
