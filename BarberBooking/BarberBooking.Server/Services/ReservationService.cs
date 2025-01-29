using BarberBooking.Server.Entities;
using BarberBooking.Server.Helper.Email;
using BarberBooking.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;

namespace BarberBooking.Server.Services
{
    public class ReservationService : IReservationsService
    {
        private readonly BarberBookingContext _db;
        private readonly IEmailService _emailService;
        private readonly ISmsService _smsService;
        public ReservationService(BarberBookingContext db ,IEmailService emailService, ISmsService smsService )
        {
            _db = db;
            _emailService = emailService;
            _smsService = smsService;
        }

        private bool IsReservationBooked(DateTime newReservation)
        {
            // check is that time currently avaiable
            return _db.Reservations.Any(r =>
                r.DateOfReservation.Date == newReservation.Date &&
                r.DateOfReservation.Hour == newReservation.Hour &&
                r.DateOfReservation.Minute == newReservation.Minute
            );
        }
        private async Task SaveReservation(int creatorId,int serviceTypeId, DateTime newReservationDateTime)
        {
            var reservation = new Reservation()
            {
                ServiceTypeId = serviceTypeId,
                UserId = creatorId,
                DateOfReservation = newReservationDateTime,
            };

            await _db.Reservations.AddAsync(reservation);
            await _db.SaveChangesAsync();
        }

        public async Task CreateReservation(int creatorId, NewReservation newReservation)
        {
            
            DateTime newReservationDatelocalTime = newReservation.DateOfReservation.ToLocalTime();

            if(newReservationDatelocalTime < DateTime.Now)
            {
                throw new ArgumentException("Time of reservation cannot be in past");
            }  

            if (IsReservationBooked(newReservationDatelocalTime))
            {
                throw new Exception("Date of reservation already exists, please choose other one");
            }
            await SaveReservation(creatorId, newReservation.ServiceTypeId, newReservationDatelocalTime);

            // redirecting to oauth page
            //return _accountService.OAuthRedirect();
            string calendarLink = GenerateCalendarLink(newReservation.DateOfReservation, newReservation.DateOfEndingService);
            string body = GenerateCalendarBody(calendarLink);
            try
            {
                this._smsService.SendSms("+381665179976", "Test sms");
                await this._emailService.SendEmail("aleksapetrovicoffice@gmail.com", "Succesfull booked service", body);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string GenerateCalendarLink(DateTime startingDate, DateTime endingDate)
        {
            return $@"https://www.google.com/calendar/render?action=TEMPLATE" +
                               $"&text={Uri.EscapeDataString("Add booking to calendar")}" +
                               $"&dates={startingDate.ToUniversalTime():yyyyMMddTHHmmssZ}/{endingDate.AddMinutes(30).ToUniversalTime():yyyyMMddTHHmmssZ}" +
                               $"&details={Uri.EscapeDataString("descrption")}" +
                               $"&location={Uri.EscapeDataString("Pariske komune 23, Novi Beograd")}";
        }

        private string GenerateCalendarBody(string calendarLink)
        {
            return 
                $@"<p>Dear Customer,</p>
                    <p>Your reservation is confirmed. Click below to add it to your calendar:</p>
                        <ul>
                            <li><a href='{calendarLink}'>Add to Google Calendar</a></li>
                        </ul>";
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

            if (IsReservationBooked(formatedDateOfReservation))
            {
                throw new ArgumentException("Reservation with provided date is already booked. Please choose other one");
            }

            existingReservation.ServiceTypeId = newReservation.ServiceTypeId;
            existingReservation.DateOfReservation = formatedDateOfReservation;
            // validation for date
            _db.Reservations.Update(existingReservation);
            await _db.SaveChangesAsync();

        }
    }
}
