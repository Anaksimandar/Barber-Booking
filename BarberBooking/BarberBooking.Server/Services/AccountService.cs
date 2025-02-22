using BarberBooking.Server.Entities;
using BarberBooking.Server.Helper.Authentication;
using BarberBooking.Server.Helper.Email;
using BarberBooking.Server.Helper.Validators;
using BarberBooking.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
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
            if (loginUser == null)
            {
                throw new ArgumentException("Login data is not provided");
            }

            User? user = await _db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == loginUser.Email);

            Validator.LoginUserValidator(loginUser, user);

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
            Validator.RegisterUserValidator(user);

            string encryptedPassword = new PasswordHasher<string>().HashPassword(null,user.Password);

            User newUser = new User()
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Number = user.Number,
                Password = encryptedPassword,
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

            Validator.SamePassword(changePasswordResponse.OldPassword, user.Password);

            user.Password = new PasswordHasher<string>().HashPassword(null, changePasswordResponse.NewPassword);

            await _db.SaveChangesAsync();
        }

        public async Task ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            User? user = await _db.Users.FirstOrDefaultAsync(
                u => u.Id == resetPasswordDto.UserId && u.PasswordToken == resetPasswordDto.PasswordToken);

            if(user == null)
            {
                throw new ArgumentException("Unable to reset password");
            }

            Validator.SamePassword(resetPasswordDto.NewPassword, resetPasswordDto.ConfirmNewPassword);

            user.Password = user.Password = new PasswordHasher<string>().HashPassword(null, resetPasswordDto.NewPassword);

            user.PasswordToken = null;

            await _db.SaveChangesAsync();
        }

        public string OAuthRedirect()
        {
            string credentialsFile = "C:\\Users\\acode\\Source\\Repos\\Barber-Booking\\BarberBooking\\BarberBooking.Server\\Files\\credentials.json";
            GoogleCredentialsModel? credentialsObject = JsonSerializer.Deserialize<GoogleCredentialsModel>(File.ReadAllText(credentialsFile));

            string googleAuthUrl = $@"https://accounts.google.com/o/oauth2/v2/auth?
                    scope=https://www.googleapis.com/auth/calendar&
                    access_type=offline&
                    response_type=code&
                    client_id={credentialsObject!.ClientId}&
                    redirect_uri=http://localhost:5137/api/auth/google&
                    state=YOUR_RANDOM_STATE_VALUE";

            return googleAuthUrl;
        }


        //public async Task AccessToken(string code)
        //{
        //    string tokensFile = "C:\\Users\\acode\\Source\\Repos\\Barber-Booking\\BarberBooking\\BarberBooking.Server\\Files\\tokens.json";
        //    string credentialsFile = "C:\\Users\\acode\\Source\\Repos\\Barber-Booking\\BarberBooking\\BarberBooking.Server\\Files\\credentials.json";
        //    GoogleCredentialsModel? credentialsObject = JsonSerializer.Deserialize<GoogleCredentialsModel>(File.ReadAllText(credentialsFile));

        //    var tokenRequest = new Dictionary<string, string>{
        //        { "code", code },
        //        { "client_id", credentialsObject!.ClientId},
        //        { "client_secret", credentialsObject.ClientSecret },
        //        { "redirect_uri", "http://localhost:5137/api/auth/google" },
        //        { "grant_type", "authorization_code" }
        //    };

        //    var content = new FormUrlEncodedContent(tokenRequest);
        //    HttpClient client = _httpClientFactory.CreateClient();
        //    var response = await client.PostAsync("https://oauth2.googleapis.com/token", content);

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        throw new Exception("Error retrieving access token");
        //    }

        //    File.WriteAllText(tokensFile, await response.Content.ReadAsStringAsync());
        //}

        //public async Task RefreshToken()
        //{
        //    string tokensFile = "C:\\Users\\acode\\Source\\Repos\\Barber-Booking\\BarberBooking\\BarberBooking.Server\\Files\\tokens.json";
        //    string credentialsFile = "C:\\Users\\acode\\Source\\Repos\\Barber-Booking\\BarberBooking\\BarberBooking.Server\\Files\\credentials.json";
        //    GoogleTokensModel? googleTokensModel = JsonSerializer.Deserialize<GoogleTokensModel>(File.ReadAllText(tokensFile));
        //    GoogleCredentialsModel? credentialsObject = JsonSerializer.Deserialize<GoogleCredentialsModel>(File.ReadAllText(credentialsFile));
        //    string refreshTokenUrl = "https://oauth2.googleapis.com/token";

        //    if (credentialsObject == null || googleTokensModel == null)
        //    {
        //        throw new Exception("Credentials or token is invalid");
        //    }

        //    var requestObject = new Dictionary<string, string>
        //    {
        //        {"client_id",credentialsObject.ClientId },
        //        {"client_secret",credentialsObject.ClientSecret },
        //        {"refresh_token",googleTokensModel.RefreshToken },
        //        {"grant_type","refresh_token"}

        //    };

        //    HttpClient client = _httpClientFactory.CreateClient();
        //    var body = new FormUrlEncodedContent(requestObject);

        //    var response = await client.PostAsync(refreshTokenUrl, body);

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        throw new Exception("Error with requesting new access token");
        //    }

        //    GoogleRefreshTokenResponse? refreshTokenResponse = JsonSerializer.Deserialize<GoogleRefreshTokenResponse>(await response.Content.ReadAsStringAsync());

        //    if (refreshTokenResponse == null) {
        //        throw new Exception("Refresh token not valid");
        //    }

        //    googleTokensModel.AccessToken = refreshTokenResponse!.AccessToken;
        //    googleTokensModel.ExpiresIn = refreshTokenResponse.ExpiresIn;
        //    googleTokensModel.Scope = refreshTokenResponse.Scope;
        //    googleTokensModel.TokenType = refreshTokenResponse.TokenType;

        //    string newGoogleTokenModel = JsonSerializer.Serialize<GoogleTokensModel>(googleTokensModel);

        //    File.WriteAllText(tokensFile, newGoogleTokenModel);
        //}

        //public async Task RevokeAccess()
        //{
        //    string tokenFile = "C:\\Users\\acode\\Source\\Repos\\Barber-Booking\\BarberBooking\\BarberBooking.Server\\Files\\tokens.json";
        //    string jsonTokenFIle = File.ReadAllText(tokenFile);
        //    GoogleTokensModel? tokenObject = JsonSerializer.Deserialize<GoogleTokensModel>(jsonTokenFIle);
        //    string rewokeUrl = "https://oauth2.googleapis.com/revoke";

        //    if(tokenObject == null)
        //    {
        //        throw new Exception("Access token not found");
        //    }

        //    var requestObject = new Dictionary<string, string>
        //    {
        //        {"token",tokenObject.AccessToken }
        //    };

        //    var body = new FormUrlEncodedContent(requestObject);

        //    HttpClient client = _httpClientFactory.CreateClient();
        //    var response = await client.PostAsync(rewokeUrl, body);

        //    if(!response.IsSuccessStatusCode)
        //    {
        //        throw new Exception("Error while attempting to revoke access");

        //    }
        //}
    }
}
