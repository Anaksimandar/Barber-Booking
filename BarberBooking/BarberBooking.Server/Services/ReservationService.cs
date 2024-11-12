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

        public async Task CreateReservation(int creatorId, NewReservation newReservation)
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
                UserId = creatorId,
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


        public async Task<List<Reservation>> GetReservations(int currentUserId, RoleType currentUserRole)
        {
            var reservationsQuery = _db.Reservations.Include(r => r.User.Role).Include(r => r.ServiceType).AsQueryable();
            // if user is admind return all reservations
            if(currentUserRole == RoleType.Admin)
            {
                return await reservationsQuery.ToListAsync();
            }
            // if its not admin return just reservations that auth user created
            return await reservationsQuery.Where(r => r.User.Id == currentUserId).ToListAsync();
        }

        public async Task UpdateReservation(int creatorId,int reservationId, NewReservation newReservation)
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

            Reservation? existingReservation = await _db.Reservations.Include(r => r.User).Include(r => r.ServiceType).FirstOrDefaultAsync(r => r.Id == reservationId && r.UserId == creatorId);

            if (existingReservation == null)
            {
                throw new ArgumentException("Reservation doesnt exists");
            }

            existingReservation.ServiceTypeId = newReservation.ServiceTypeId;
            existingReservation.DateOfReservation = formatedDateOfReservation;
            // validation for date
            _db.Reservations.Update(existingReservation);
            await _db.SaveChangesAsync();

        }
    }
}
