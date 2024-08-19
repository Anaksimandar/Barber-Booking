namespace BarberBooking.Server.Entities
{
    public class Service
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        
    }
}
