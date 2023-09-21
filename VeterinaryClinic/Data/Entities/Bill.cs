using Org.BouncyCastle.Asn1.Misc;

namespace VeterinaryClinic.Data.Entities
{
    public class Bill : IEntity
    {
        public int Id { get; set; }

        public User User { get; set; }

        public Appointment Appointment { get; set; }

        public double Cost { get; set; }

    }
}
