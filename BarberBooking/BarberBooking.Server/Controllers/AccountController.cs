using BarberBooking.Server.Entities;
using BarberBooking.Server.Helper.Exceptions;
using BarberBooking.Server.Models;
using BarberBooking.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.IO;
namespace BarberBooking.Server.Controllers
{
    [Route("api")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IHttpClientFactory _httpClientFactory;
        public AccountController(IAccountService accountService, IHttpClientFactory httpClientFactory)
        {
            _accountService = accountService;
            _httpClientFactory = httpClientFactory;
        }

        [Authorize]
        [HttpGet("all-users")]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            var users = await this._accountService.GetUsers();
            return Ok(users);
        }
        [HttpPost("auth/google/revoke")]
        public async Task<IActionResult> RevokeAccess()
        {
            try
            {
                await this._accountService.RevokeAccess();
                return Ok();
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("auth/google/refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                await _accountService.RefreshToken();

                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("auth/google")]
        public async Task<IActionResult> Redirect([FromQuery] string code, [FromQuery] string state)
        {
            
            await _accountService.AccessToken(code);
            
            return Redirect("/");
            
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginUser user)
        {
            LoginResponse loginResponse;
            try
            {
                loginResponse = await _accountService.Login(user);
            }
            catch (UnauthorizedAccessException ex) { 
                return Unauthorized(ex);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(loginResponse);
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> Register(UserRegistration user)
        {
            try
            {
                await _accountService.Register(user);
            }
            catch(ArgumentException ex)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordResponse changePasswordResponse)
        {
            try
            {
                await this._accountService.ChangePassword(changePasswordResponse);
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }

            return Ok("Password has been changes successfully");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            try
            {
                await _accountService.ResetPassword(resetPasswordDto);
            }
            catch(ArgumentException ex) // passwords are not same
            { 
                return NotFound(ex.Message);
            }
            catch(EmailTokenException ex)
            {
                return BadRequest(ex.Message); // password token or user id doesnt belong to no user
            }

            return Ok();
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string mail)
        {
            try
            {
                await _accountService.ForgotPassword(mail);
            }
            catch(EmailServiceException ex)
            {
                return NotFound(ex.Message);
            }
            return Ok();
        }
    }
}
