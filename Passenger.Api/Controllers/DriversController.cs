using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using Passenger.Infrastructure.Commands;
using Passenger.Infrastructure.Commands.Drivers;
using Passenger.Infrastructure.Services;

namespace Passenger.Api.Controllers
{
    public class DriversController : ApiControllerBase
    {
        // pobieranie logera zgodnie z konwencją bez wstrzykiwania zależności
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
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
            Logger.Info("Fetching drivers.GetType");
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
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateDriver command)
        {
            await DispatchAsync(command);
            //Location: drivers/driverID
            return Created($"drivers/{command.UserId}",new object());
        }

        [Authorize]
        [HttpPut("me")] // aktualizuje drivera - akzutalizuje moje konto kierowcy
        public async Task<IActionResult> Put([FromBody]UpdateDriver command)
        {
            await DispatchAsync(command);
            //Location: drivers/driverID
            return NoContent();
        }

        [Authorize]
        [HttpDelete("me")] // usuwa instancje  drivera skojarzona z zalogownaym uzytkownikiem
        public async Task<IActionResult> Post()
        {
            await DispatchAsync(new DeleteDriver());
            //Location: drivers/driverID
            return NoContent();
        }

    }
}
