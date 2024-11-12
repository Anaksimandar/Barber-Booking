using BarberBooking.Server.Entities;

namespace BarberBooking.Server.Models
{
    public class AuthenticatedUser
    {
        public string Name { get; set;}
        public string Surname { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
    }
}
