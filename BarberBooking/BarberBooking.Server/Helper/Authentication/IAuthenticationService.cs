namespace BarberBooking.Server.Helper.Authentication
{
    public interface IAuthenticationService
    {
        string CreateJwtToken(string mail);
    }
}
