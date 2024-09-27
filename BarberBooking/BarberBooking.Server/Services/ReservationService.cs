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

        public async Task CreateReservation(NewReservation newReservation)
        {
            var reservation = new Reservation()
            {
                ServiceTypeId = newReservation.ServiceTypeId,
                UserId = newReservation.UserId,
                DateOfReservation = newReservation.DateOfReservation
            };
            await _db.Reservations.AddAsync(reservation);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Reservation>> GetReservations()
        {
            var reservations = await _db.Reservations.Include(r => r.ServiceType)
                .ToListAsync();

            return reservations;
        }
    }
}
