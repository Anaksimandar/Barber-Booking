namespace BarberBooking.Server.Helper.Email
{
    public interface IEmailService
    {
        Task SendEmail(string email, string subject, string body);
    }
}
