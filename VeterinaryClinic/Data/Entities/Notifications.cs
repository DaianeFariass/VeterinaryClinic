namespace VeterinaryClinic.Data.Entities
{
    public class Notifications : IEntity
    {
        public int Id { get; set; }

        public Appointment Appointment { get; set; }

        public NotificationTypes NotificationTypes { get; set;}

        public bool IsRead { get; set; }
    }
}
