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

        public async Task<Reservation> CreateReservation(int serviceTypeId)
        {
            var createdReservation = new Reservation()
            {
                UserId = 1,
                ServiceTypeId = serviceTypeId
            };
            await _db.Reservations.AddAsync(createdReservation);
            await _db.SaveChangesAsync();

            return createdReservation;
        }

        public async Task<List<Reservation>> GetReservations()
        {
            var reservations = await _db.Reservations.Include(r => r.ServiceTypeId)
                .ToListAsync();

            return reservations;
        }
    }
}
