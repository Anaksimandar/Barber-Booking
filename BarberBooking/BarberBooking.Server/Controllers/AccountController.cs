using BarberBooking.Server.Models;
using BarberBooking.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BarberBooking.Server.Controllers
{
    [Route("api")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService) {
            _accountService = accountService;
        }
        [HttpPost("login")]
        public async Task<LoginResponse> Login(LoginUser user)
        {
            LoginResponse loginResponse = await _accountService.Login(user);

            return loginResponse;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistration user)
        {
            await _accountService.Register(user);

            return Ok();
        }
    }
}
