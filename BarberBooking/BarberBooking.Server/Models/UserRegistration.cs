namespace BarberBooking.Server.Models
{
    public class UserRegistration
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public int RoleId { get; set; } = 1; // initial role will be user = 0
    }
}
