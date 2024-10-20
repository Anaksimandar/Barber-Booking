namespace BarberBooking.Server.Entities
{
    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }

    }
}
