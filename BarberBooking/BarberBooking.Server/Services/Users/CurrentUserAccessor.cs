using BarberBooking.Server.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BarberBooking.Server.Services.Users
{
    public class CurrentUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BarberBookingContext _db;
        public CurrentUserAccessor(IHttpContextAccessor httpConextAccessor, BarberBookingContext db ) {
            this._httpContextAccessor = httpConextAccessor;
            this._db = db;
        }

        public User GetCurrentUser()
        {
            if (this._httpContextAccessor.HttpContext?.User.Identity == null || !this._httpContextAccessor.HttpContext.User.Identity.IsAuthenticated) {
                throw new Exception("User is not authenticated");
            }

            var emailClaim = this._httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email);

            if (emailClaim == null)
            {
                throw new Exception("Missing email claim");
            }

            var user = _db.Users
                .AsNoTracking()
                .Include(u => u.Role)
                .FirstOrDefault(x => x.Email == emailClaim.Value);

            if(user == null)
            {
                throw new Exception("User doesnt exists");
            }

            return user;
        }
    }
}
