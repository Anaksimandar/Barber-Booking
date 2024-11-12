using BarberBooking.Server.Entities;
using BarberBooking.Server.Models;

namespace BarberBooking.Server.Services
{
    public interface IReservationsService
    {
        Task<List<Reservation>> GetReservations(int currentUserId, RoleType currenUserRole);
        Task CreateReservation(int creatorId,NewReservation newReservation);
        Task<bool> DeleteReservation(int reservationId);
        Task UpdateReservation(int creatorId,int reservationId, NewReservation newReservation);
    }
}
