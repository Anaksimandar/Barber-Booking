namespace BarberBooking.Server.Models
{
    public class ResetPasswordDto
    {
        public int UserId { get;set; }
        public string PasswordToken { get;set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
