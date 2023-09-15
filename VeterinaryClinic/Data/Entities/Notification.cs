
using VeterinaryClinic.Enums;

namespace VeterinaryClinic.Data.Entities
{
    public class Notification : IEntity
    {
        public int Id { get; set; }

        public Appointment Appointment { get; set; }

        public NotificationTypes NotificationTypes { get; set;}

       
    }
}
