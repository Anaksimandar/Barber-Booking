using BarberBooking.Server.Entities;
using BarberBooking.Server.Models;
using BarberBooking.Server.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace BarberBooking.Server.Controllers
{
    public class CurrentUserController : ControllerBase
    {
        private readonly CurrentUserAccessor _currentUserAccessor;
        public CurrentUserController(CurrentUserAccessor currentUserAccessor) {
            this._currentUserAccessor = currentUserAccessor;
        }

        protected User GetCurrentUser()
        {
            return _currentUserAccessor.GetCurrentUser();
        }
    }
}
