using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Passenger.Infrastructure.Commands;
using Passenger.Infrastructure.Commands.Drivers;
using Passenger.Infrastructure.Services;

namespace Passenger.Api.Controllers
{
    public class DriversController : ApiControllerBase
    {
        private readonly IDriverService _driverService;
        public DriversController(
            IDriverService driverService,
            ICommandDispatcher commandDispatcher) : base(commandDispatcher)
        {
            _driverService = driverService;
        }

        // zwracamy wszystkich driverow
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var drivers = await _driverService.BrowseAsync();
            return Json(drivers);
        }

        // GET drivers/{userId}
        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(string userId)
        {
            var driver = await _driverService.GetAsync(Guid.Parse(userId));
            if(driver == null)
            {
                return NotFound();
            }

            return Json(driver);
        }
        
        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]CreateDriver command)
        {
            await DispatchAsync(command);
            //Location: drivers/driverID
            return Created($"drivers/{command.UserId}",new object());
        }

    }
}
