using BarberBooking.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace BarberBooking.Server.Services
{
    public class ReservationService : IReservationsService
    {
        private readonly BarberBookingContext _db;

        public ReservationService(BarberBookingContext db)
        {
            _db = db;
        }
        public async Task<List<Reservation>> GetReservations()
        {
            var reservations = await _db.Reservations.ToListAsync();

            return reservations;
        }
    }
}
