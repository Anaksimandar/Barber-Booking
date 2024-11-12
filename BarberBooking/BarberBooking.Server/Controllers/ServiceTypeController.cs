using BarberBooking.Server.Entities;
using BarberBooking.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberBooking.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/service-type")]
    public class ServiceTypeController : ControllerBase
    {
        private readonly IServiceTypeService _serviceTypeService;
        public ServiceTypeController(IServiceTypeService serviceTypeService) {
            _serviceTypeService = serviceTypeService;
        }
        [HttpGet]
        public async Task<ActionResult<List<ServiceType>>> GetAllServiceTypes()
        {
            var serviceType = await _serviceTypeService.GetServiceTypes();

            return Ok(serviceType);

            
        }
    }
}
