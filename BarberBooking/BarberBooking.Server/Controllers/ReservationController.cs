using BarberBooking.Server.Models;
using BarberBooking.Server.Services;
using BarberBooking.Server.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberBooking.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/reservation")]
    public class ReservationController: CurrentUserController
    {
        private readonly IReservationsService _reservationsService;

        public ReservationController(IReservationsService reservationsService, CurrentUserAccessor currentUserAcessor):base(currentUserAcessor)
        {
            _reservationsService = reservationsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetReservations()
        {
            try
            {
                var currentUser = GetCurrentUser();
                var reservations = await _reservationsService.GetReservations(currentUser.Id, currentUser.Role.Name);
                return Ok(reservations);
            }
            catch (Exception ex)
            {
                if (ex.Message == "User is not authenticated" || ex.Message == "Missing email claim")
                {
                    return Unauthorized(new { message = "User is not authenticated. Please log in." });
                }
                
                // Handle other exceptions
                return StatusCode(500, "An error occurred while retrieving reservations.");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateReservation([FromBody] NewReservation newReservation)
        {
            try
            {
                var currentUser = GetCurrentUser();
                await _reservationsService.CreateReservation(currentUser.Id, currentUser.Number ,newReservation);
                
                return Ok(new { Message="Confirmation link has been send to " + currentUser.Email });
            }
            catch(Exception ex){
                if (ex.Message == "User is not authenticated" || ex.Message == "Missing email claim")
                {
                    return Unauthorized(new { Message = ex.Message });
                }
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{reservationId}")]
        public async Task<IActionResult> DeleteReservation(int reservationId)
        {
            try
            {
                await _reservationsService.DeleteReservation(reservationId);
                return Ok();
            }
            catch (Exception ex) { 
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{reservationId}")]
        public async Task<IActionResult> UpdateReservation(int reservationId,[FromBody] NewReservation newReservation)
        {
            
            try
            {
                var currentUser = GetCurrentUser();
                await _reservationsService.UpdateReservation(currentUser.Id, reservationId, newReservation);

                return Ok();
            }
            catch (ArgumentException argErr)
            {
                return BadRequest(new {message = argErr.Message});
            }

        }
    }
}
