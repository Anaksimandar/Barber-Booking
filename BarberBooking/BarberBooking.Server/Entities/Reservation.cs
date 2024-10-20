using System.ComponentModel.DataAnnotations.Schema;

namespace BarberBooking.Server.Entities
{
    public class Reservation
    {
        public int Id { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public User User { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<Service> Services { get; set; }

    }
}
