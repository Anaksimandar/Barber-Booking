using BarberBooking.Server.Entities;
using BarberBooking.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace BarberBooking.Server.Controllers
{
    [ApiController]
    [Route("api/reservation")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationsService _reservationsService;

        public ReservationController(IReservationsService reservationsService)
        {
            _reservationsService = reservationsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetReservations() 
        { 
            var reservations = await _reservationsService.GetReservations();
            return Ok(reservations);
        }

        [HttpPost]
        public async Task<ActionResult> CreateReservation([FromBody] NewReservation newReservation)
        {
            await _reservationsService.CreateReservation(newReservation);

            //return CreatedAtAction(nameof(CreateReservation), new { id = newReservation.Id });

            return Ok();
        }
    }
}
