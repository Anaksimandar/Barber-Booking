using BarberBooking.Server.Entities;

namespace BarberBooking.Server.Services
{
    public interface ISeedingService
    {
        Task SeedUsers();
        Task SeedReservations();
        Task SeedServices();
    }
}
