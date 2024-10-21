namespace BarberBooking.Server.Models
{
    public class LoginResponse
    {
        public LoginUser? LoginUser { get; set; }
        public string? Token { get; set; }
    }
}
