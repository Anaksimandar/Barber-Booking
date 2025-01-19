namespace BarberBooking.Server.Models
{
    public class ChangePasswordResponse
    {
        public string Mail { get; set; } = string.Empty;
        public string OldPassword { get; set; } = String.Empty;
        public string NewPassword { get; set; } = String.Empty;
    }
}
