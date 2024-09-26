using System.ComponentModel.DataAnnotations.Schema;

namespace BarberBooking.Server.Entities
{
    public class ServiceType
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int Duration { get; set; }
        public double Price { get; set; }
    }
}
