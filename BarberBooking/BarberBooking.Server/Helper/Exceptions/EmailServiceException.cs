namespace BarberBooking.Server.Helper.Exceptions
{
    public class EmailServiceException : Exception
    {
        public EmailServiceException(string message):base(message) { }
    }
}
