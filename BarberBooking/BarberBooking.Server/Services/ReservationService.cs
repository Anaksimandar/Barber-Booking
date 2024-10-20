using BarberBooking.Server.Entities;
using BarberBooking.Server.Models;
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
            DateTime formatedDateOfReservation = newReservation.DateOfReservation.ToLocalTime();
            if(formatedDateOfReservation < DateTime.Now)
            {
                throw new ArgumentException("Time of reservation cannot be in past");
            }
            var reservation = new Reservation()
            {
                ServiceTypeId = newReservation.ServiceTypeId,
                UserId = newReservation.UserId,
                DateOfReservation = formatedDateOfReservation,
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
            DateTime formatedDateOfReservation = newReservation.DateOfReservation.ToLocalTime();

            /*
             * we shoud check doest reservation with provided resId have userId of auth user.
             * Why ? Because if we dont check it means anyone could just add rndres id and change date of that reservation
            */
            if (formatedDateOfReservation < DateTime.Now)
            {
                throw new ArgumentException("Time of reservation cannot be in past");
            }

            bool reservationExists =  _db.Reservations.Where(r => r.Id == reservationId).Any();

            if (!reservationExists)
            {
                throw new ArgumentException("Reservation doesnt exists");
            }
            Reservation reservation = new Reservation()
            {
                Id = reservationId,
                UserId = newReservation.UserId,
                DateOfReservation = formatedDateOfReservation,
                ServiceTypeId = newReservation.ServiceTypeId
            };
            // validation for date
            _db.Reservations.Update(reservation);
            await _db.SaveChangesAsync();

        }
    }
}
