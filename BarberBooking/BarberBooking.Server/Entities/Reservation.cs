using BarberBooking.Server.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarberBooking.Server.Entities
{
    public class Reservation
    {
        public int Id { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("ServiceTypeId")]
        public int ServiceTypeId { get; set; }
        public ServiceType ServiceType { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [NotMapped]
        public DateTime EndingAt
        {
            get
            {
                if(ServiceType != null)
                {
                    CreatedAt.AddMinutes(ServiceType.Duration);
                }

                return CreatedAt;
            }
        }



    }
}
