using BarberBooking.Server.Entities;
using BarberBooking.Server.Helper.Authentication;
using BarberBooking.Server.Helper.Email;
using BarberBooking.Server.Helper.Exceptions;
using BarberBooking.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BarberBooking.Server.Services
{
    public class AccountService : IAccountService
    {
        private readonly BarberBookingContext _db;
        private readonly IAuthenticationService _authenticationService;
        private readonly IEmailService _emailService;
        private readonly IHttpClientFactory _httpClientFactory;
        public AccountService(
            BarberBookingContext db,
            IAuthenticationService authenticationService,
            EmailService emailService,
            IHttpClientFactory httpClientFactory
        ){
            _httpClientFactory = httpClientFactory;
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
            // check does old password exits
            User? user = await this._db.Users.FirstOrDefaultAsync(u => u.Email == changePasswordResponse.Mail);
            if (user == null) {
                throw new ArgumentException("User doesnt exists");
            }

            if(user.Password != changePasswordResponse.OldPassword)
            {
                throw new ArgumentException("Please enter correct old password");
            }

            user.Password = changePasswordResponse.NewPassword;
            await _db.SaveChangesAsync();
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

        public string OAuthRedirect()
        {
            string googleAuthUrl = "https://accounts.google.com/o/oauth2/v2/auth?" +
                    "scope=https://www.googleapis.com/auth/calendar&" +
                    "access_type=offline&" +
                    "response_type=code&" +
                    "client_id=426884900522-pqhn7ati50jkk6tfb4ouja8vkg6tld66.apps.googleusercontent.com&" +
                    "redirect_uri=http://localhost:5137/api/auth/google&" +
                    "state=YOUR_RANDOM_STATE_VALUE";

            return googleAuthUrl;
        }


        public async Task AccessToken(string code)
        {
            string tokenFile = "C:\\Users\\acode\\Source\\Repos\\Barber-Booking\\BarberBooking\\BarberBooking.Server\\Files\\tokens.json";
            var tokenRequest = new Dictionary<string, string>{
                { "code", code },
                { "client_id", "426884900522-pqhn7ati50jkk6tfb4ouja8vkg6tld66.apps.googleusercontent.com" },
                { "client_secret", "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" },
                { "redirect_uri", "http://localhost:5137/api/auth/google" },
                { "grant_type", "authorization_code" }
            };

            var content = new FormUrlEncodedContent(tokenRequest);
            HttpClient client = _httpClientFactory.CreateClient();
            var response = await client.PostAsync("https://oauth2.googleapis.com/token", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error retrieving access token");
            }

            File.WriteAllText(tokenFile, await response.Content.ReadAsStringAsync());
        }

        public async Task RefreshToken()
        {
            string tokensFile = "C:\\Users\\acode\\Source\\Repos\\Barber-Booking\\BarberBooking\\BarberBooking.Server\\Files\\tokens.json";
            string credentialsFile = "C:\\Users\\acode\\Source\\Repos\\Barber-Booking\\BarberBooking\\BarberBooking.Server\\Files\\credentials.json";
            GoogleTokensModel? googleTokensModel = JsonSerializer.Deserialize<GoogleTokensModel>(File.ReadAllText(tokensFile));
            GoogleCredentialsModel? credentialsObject = JsonSerializer.Deserialize<GoogleCredentialsModel>(File.ReadAllText(credentialsFile));
            string refreshTokenUrl = "https://oauth2.googleapis.com/token";

            if (credentialsObject == null || googleTokensModel == null)
            {
                throw new Exception("Credentials or token is invalid");
            }

            var requestObject = new Dictionary<string, string>
            {
                {"client_id",credentialsObject.ClientId },
                {"client_secret",credentialsObject.ClientSecret },
                {"refresh_token",googleTokensModel.RefreshToken },
                {"grant_type","refresh_token"}

            };

            HttpClient client = _httpClientFactory.CreateClient();
            var body = new FormUrlEncodedContent(requestObject);

            var response = await client.PostAsync(refreshTokenUrl, body);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error with requesting new access token");
            }

            GoogleRefreshTokenResponse? refreshTokenResponse = JsonSerializer.Deserialize<GoogleRefreshTokenResponse>(await response.Content.ReadAsStringAsync());

            if (refreshTokenResponse == null) {
                throw new Exception("Refresh token not valid");
            }

            googleTokensModel.AccessToken = refreshTokenResponse!.AccessToken;
            googleTokensModel.ExpiresIn = refreshTokenResponse.ExpiresIn;
            googleTokensModel.Scope = refreshTokenResponse.Scope;
            googleTokensModel.TokenType = refreshTokenResponse.TokenType;

            string newGoogleTokenModel = JsonSerializer.Serialize<GoogleTokensModel>(googleTokensModel);

            File.WriteAllText(tokensFile, newGoogleTokenModel);
        }

        public async Task RevokeAccess()
        {
            string tokenFile = "C:\\Users\\acode\\Source\\Repos\\Barber-Booking\\BarberBooking\\BarberBooking.Server\\Files\\tokens.json";
            string jsonTokenFIle = File.ReadAllText(tokenFile);
            GoogleTokensModel? tokenObject = JsonSerializer.Deserialize<GoogleTokensModel>(jsonTokenFIle);
            string rewokeUrl = "https://oauth2.googleapis.com/revoke";

            if(tokenObject == null)
            {
                throw new Exception("Access token not found");
            }

            var requestObject = new Dictionary<string, string>
            {
                {"token",tokenObject.AccessToken }
            };

            var body = new FormUrlEncodedContent(requestObject);

            HttpClient client = _httpClientFactory.CreateClient();
            var response = await client.PostAsync(rewokeUrl, body);

            if(!response.IsSuccessStatusCode)
            {
                throw new Exception("Error while attempting to revoke access");

            }
        }
    }
}
