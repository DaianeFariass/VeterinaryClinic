using System.ComponentModel.DataAnnotations;

namespace VeterinaryClinic.Data.Entities
{
    public class Room : IEntity
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Room")]
        public string RoomNumber { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }

        public Vet Vet { get; set; }

        public User User { get; set; }
    }
}
