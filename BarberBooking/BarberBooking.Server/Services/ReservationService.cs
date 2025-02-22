using BarberBooking.Server.Entities;
using BarberBooking.Server.Helper.Email;
using BarberBooking.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using BarberBooking.Server.Services.Sms;

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

        public async Task CreateReservation(int creatorId, string number, NewReservation newReservation)
        {
            
            DateTime newReservationDatelocalTime = FormatedDateTime(newReservation.DateOfReservation);

            if(IsDateTimeInPast(newReservationDatelocalTime))
            {
                throw new ArgumentException("Time of reservation cannot be in past");
            }  

            if (IsReservationBooked(newReservationDatelocalTime))
            {
                throw new Exception("Date of reservation already exists, please choose other one");
            }

            await SaveReservation(creatorId, newReservation.ServiceTypeId, newReservationDatelocalTime);

            try
            {
                await SendCalendarReminder(newReservation);
                await SendSmsConfirmation(number, "Your reservation has been created succesfully");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task SendSmsConfirmation(string number, string message)
        {
            await _smsService.SendConfirmation(number, message);

        }

        private async Task SendCalendarReminder(NewReservation newReservation)
        {
            string calendarLink = GenerateCalendarLink(newReservation.DateOfReservation, newReservation.DateOfEndingService);
            string body = GenerateCalendarBody(calendarLink);
            await this._emailService.SendEmail("aleksapetrovicoffice@gmail.com", "Succesfull booked service", body);
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

        public async Task DeleteReservation(int reservationId)
        {
            var reservation = await _db.Reservations.FindAsync(reservationId);

            if (reservation == null)
            {
                throw new Exception("Reservation not found");
            }

            await DeleteReservation(reservation);  
        }

        private async Task DeleteReservation(Reservation reservation)
        {
            _db.Reservations.Remove(reservation);

            await _db.SaveChangesAsync();
        }

        private bool IsDateTimeInPast(DateTime date)
        {
            return date < DateTime.Now;
        }

        private DateTime FormatedDateTime(DateTime dateTime)
        {
            return dateTime.ToLocalTime();
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
            DateTime formatedDateTime = FormatedDateTime(newReservation.DateOfReservation);
            /*
             * we shoud check doest reservation with provided resId have userId of auth user.
             * Why ? Because if we dont check it means anyone could just add rnd res_id and change date of that reservation
            */
            if (IsDateTimeInPast(formatedDateTime))
            {
                throw new ArgumentException("Time of reservation cannot be in past");
            }

            // checking can user try to change reservation that isnt his own.
            List<Reservation> reservations = _db.Reservations
                .Where(r => r.UserId == creatorId).ToList();

            Reservation? existingReservation = reservations.FirstOrDefault(r => r.Id == reservationId);

            if (existingReservation == null)
            {
                throw new ArgumentException("Reservation doesnt exists");
            }

            // current date time is already booked by other reservation (for current is allowed to confirmed to book again)
            bool isAllowedToEdit = reservations.Any(r =>
                r.UserId == creatorId &&
                r.Id == reservationId &&
                r.DateOfReservation.Date == newReservation.DateOfReservation.Date &&
                r.DateOfReservation.Hour == newReservation.DateOfReservation.Hour &&
                r.DateOfReservation.Minute == newReservation.DateOfReservation.Minute
                
            );

            if (isAllowedToEdit)
            {
                throw new ArgumentException("Date already exists");
            }

            existingReservation.ServiceTypeId = newReservation.ServiceTypeId;
            existingReservation.DateOfReservation = formatedDateTime;
            // validation for date
            _db.Reservations.Update(existingReservation);
            await _db.SaveChangesAsync();

        }


    }
}
