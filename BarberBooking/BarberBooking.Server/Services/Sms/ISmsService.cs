namespace BarberBooking.Server.Services.Sms
{
    public interface ISmsService
    {
        public Task SendConfirmation(string to, string text);
        public Task SendReminder(string to);
    }
}
