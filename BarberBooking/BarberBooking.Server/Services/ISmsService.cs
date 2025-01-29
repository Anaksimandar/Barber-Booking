namespace BarberBooking.Server.Services
{
    public interface ISmsService
    {
        public void SendSms(string to, string message);
    }
}
