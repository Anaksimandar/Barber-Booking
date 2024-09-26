using BarberBooking.Server.Services;
using Microsoft.EntityFrameworkCore;

namespace BarberBooking.Server.Entities
{
    public class BarberBookingContext : DbContext
    {
        public BarberBookingContext(DbContextOptions options):base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Add your model configurations here
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.ServiceType)
                .WithMany() // No navigation property on ServiceType
                .OnDelete(DeleteBehavior.Cascade); // Specify delete behavior
        }

    }
}
