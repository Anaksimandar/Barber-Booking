namespace BarberBooking.Server.Models
{
    public class ChangePasswordResponse
    {
        public string NewPassword { get; set; } = String.Empty;
        public string ConfirmNewPassword { get; set; } = String.Empty;
    }
}
