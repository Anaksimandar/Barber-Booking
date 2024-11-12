using BarberBooking.Server.Entities;
using BarberBooking.Server.Models;
using BarberBooking.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberBooking.Server.Controllers
{
    [Authorize]
    [Route("api")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService) {
            _accountService = accountService;
        }

        [HttpGet("all-users")]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            var users = await this._accountService.GetUsers();
            return Ok(users);
        }
        [HttpPost("login")]
        public async Task<LoginResponse> Login(LoginUser user)
        {
            LoginResponse loginResponse = await _accountService.Login(user);
            return loginResponse;
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> Register(UserRegistration user)
        {
            await _accountService.Register(user);

            return Ok();
        }
    }
}
