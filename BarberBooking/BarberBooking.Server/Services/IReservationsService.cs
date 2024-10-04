using BarberBooking.Server.Entities;

namespace BarberBooking.Server.Services
{
    public interface IReservationsService
    {
        Task<List<Reservation>> GetReservations();
        Task CreateReservation(NewReservation newReservation);
        Task<bool> DeleteReservation(int reservationId);
        Task UpdateReservation(int reservationId, NewReservation newReservation);
    }
}
