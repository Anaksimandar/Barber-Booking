using BarberBooking.Server.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
            // we should check on backend is reservation date valid (is there any other reservation with same date)

            var reservation = new Reservation()
            {
                ServiceTypeId = newReservation.ServiceTypeId,
                UserId = newReservation.UserId,
                DateOfReservation = newReservation.DateOfReservation.ToLocalTime(),
            };
            await _db.Reservations.AddAsync(reservation);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> DeleteReservation(int reservationId)
        {
            try
            {
                var reservation = await _db.Reservations.FindAsync(reservationId);
                if (reservation != null)
                {
                    _db.Reservations.Remove(reservation);
                    await _db.SaveChangesAsync();
                    return true; // Deletion successful
                }
                return false; // Reservation not found
            }
            catch (DbUpdateException ex)
            {
                // Handle database update exceptions
                throw new Exception("An error occurred while deleting the reservation.", ex);
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                throw new Exception("An unexpected error occurred.", ex);
            }
        }


        public async Task<List<Reservation>> GetReservations()
        {
            var reservations = await _db.Reservations.Include(r => r.ServiceType)
                .ToListAsync();

            return reservations;
        }

        public async Task UpdateReservation(int reservationId, NewReservation newReservation)
        {

            
            Reservation? reservation = await _db.Reservations.Include(r => r.ServiceType).FirstOrDefaultAsync(r => r.Id == reservationId);

            if (reservation == null)
            {
                throw new ArgumentException("Reservation doesnt exists");
            }

            reservation.DateOfReservation = newReservation.DateOfReservation.ToLocalTime();
            reservation.ServiceType.Id = newReservation.ServiceTypeId;
            // validation for date
            _db.Reservations.Update(reservation);
            await _db.SaveChangesAsync();

        }
    }
}
