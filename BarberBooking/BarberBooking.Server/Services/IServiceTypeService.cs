using BarberBooking.Server.Entities;

namespace BarberBooking.Server.Services
{
    public interface IServiceTypeService
    {
        Task<List<ServiceType>> GetServiceTypes();
    }
}
