using BarberBooking.Server.Entities;
using BarberBooking.Server.Helper.Authentication;
using BarberBooking.Server.Helper.Email;
using BarberBooking.Server.Helper.Exceptions;
using BarberBooking.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace BarberBooking.Server.Services
{
    public class AccountService : IAccountService
    {
        private readonly BarberBookingContext _db;
        private readonly IAuthenticationService _authenticationService;
        private readonly IEmailService _emailService;
        public AccountService(BarberBookingContext db, IAuthenticationService authenticationService,EmailService emailService) {
            _db = db;
            _authenticationService = authenticationService;
            _emailService = emailService;
        }

        public async Task<List<User>> GetUsers()
        {
            var users = await _db.Users.Include(u => u.Role).ToListAsync();

            return users;
        }

        public async Task<LoginResponse> Login(LoginUser loginUser)
        {
            var user = await _db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == loginUser.Email);

            if (user == null)
            {
                throw new ArgumentException("User with provided email doesn't exists");
            }

            if(user.Password != loginUser.Password)
            {
                throw new UnauthorizedAccessException("Mail or passwords are not valid");
            }
            // generating token based on email
            var token = _authenticationService.CreateJwtToken(loginUser.Email);
            // returning response object with logged in user and his token
            AuthenticatedUser authenticatedUser = new AuthenticatedUser()
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Role = user.Role,

            };

            return new LoginResponse() { User = authenticatedUser, Token = token };
        }

        public async Task Register(UserRegistration user)
        {
            if(user.Name == null || user.Surname == null || user.Password == null || user.ConfirmPassword == null || user.Email == null)
            {
                throw new ArgumentException("Please check your data");
            }
            User newUser = new User()
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Password = user.Password,
                RoleId = user.RoleId = user.RoleId
            };

            await _db.Users.AddAsync(newUser);
            await _db.SaveChangesAsync();

        }

        public async Task ForgotPassword(string mail) // this service should generate id and send on provided email
        {
            User? user = await _db.Users.FirstOrDefaultAsync(u => u.Email == mail);

            if (user == null)
            {
                throw new ArgumentException("Provided email doest exist");
            }

            // generate and add password token to user
            string passwordToken = Guid.NewGuid().ToString();
            user.PasswordToken = passwordToken;
            await _db.SaveChangesAsync();

            // send email to user (we have to make sure that mail has been send and then we have to add passwordToken to user)
            string body = $"https://localhost:4200/reset-password?userId={user.Id}&token={passwordToken}";
            await this._emailService.SendEmail(mail, "Barber Booking-Change Password", body);

            return;

        }

        

        public async Task ChangePassword(ChangePasswordResponse changePasswordResponse)
        {
            if(changePasswordResponse.NewPassword != changePasswordResponse.ConfirmNewPassword)
            {
                throw new ArgumentException("Both passwords should be same");
            }

            
        }

        public async Task ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            User? user = await _db.Users.FirstOrDefaultAsync(
                u => u.Id == resetPasswordDto.UserId && u.PasswordToken == resetPasswordDto.PasswordToken);

            if(user == null)
            {
                throw new EmailTokenException("Unable to reset password");
            }

            if(resetPasswordDto.NewPassword != resetPasswordDto.ConfirmNewPassword)
            {
                throw new ArgumentException("Passwords are not same");
            }

            user.Password = resetPasswordDto.NewPassword;
            user.PasswordToken = null;

            await _db.SaveChangesAsync();
        }
    }
}
