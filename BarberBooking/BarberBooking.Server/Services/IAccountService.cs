using BarberBooking.Server.Entities;
using BarberBooking.Server.Models;

namespace BarberBooking.Server.Services
{
    public interface IAccountService
    {
        Task Register(UserRegistration user);
        Task<LoginResponse> Login(LoginUser user);
        Task<List<User>> GetUsers();
        Task ForgotPassword(string mail);
        Task ResetPassword(ResetPasswordDto resetPasswordDto);
        Task ChangePassword(ChangePasswordResponse changePasswordResponse);
    }
}
