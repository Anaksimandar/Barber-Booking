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
        public DbSet<Role> Roles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.ServiceType)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<User>()
            //    .HasOne(u => u.Role)
            //    .WithMany()
            //    .HasForeignKey(u => u.Role)
            //    .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
