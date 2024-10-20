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
    }
}
