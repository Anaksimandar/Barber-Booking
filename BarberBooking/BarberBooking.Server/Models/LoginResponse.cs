using BarberBooking.Server.Entities;

namespace BarberBooking.Server.Models
{
    public class LoginResponse
    {
        public AuthenticatedUser? User { get; set; }
        public string Token { get; set; }
    }
}
