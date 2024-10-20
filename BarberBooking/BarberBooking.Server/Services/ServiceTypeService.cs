using BarberBooking.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace BarberBooking.Server.Services
{
    public class ServiceTypeService : IServiceTypeService
    {
        private readonly BarberBookingContext _db;
        public ServiceTypeService(BarberBookingContext db) { 
            _db = db;
        }

        public Task<List<ServiceType>> GetServiceTypes()
        {
            return _db.ServiceTypes.ToListAsync();
        }
    }
}
