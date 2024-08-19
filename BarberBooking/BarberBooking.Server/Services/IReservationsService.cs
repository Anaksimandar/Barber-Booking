using BarberBooking.Server.Entities;

namespace BarberBooking.Server.Services
{
    public interface IReservationsService
    {
        Task<List<Reservation>> GetReservations();
    }
}
