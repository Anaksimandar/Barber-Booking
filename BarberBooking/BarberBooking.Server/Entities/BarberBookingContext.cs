using Microsoft.EntityFrameworkCore;

namespace BarberBooking.Server.Entities
{
    public class BarberBookingContext : DbContext
    {
        public BarberBookingContext(DbContextOptions options):base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Service> Services { get; set; }
    }
}
