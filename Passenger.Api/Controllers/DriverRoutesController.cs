using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Passenger.Infrastructure.Commands;
using Passenger.Infrastructure.Commands.Drivers;

namespace Passenger.Api.Controllers
{
    [Route("drivers/routes")]
    public class DriverRoutesController : ApiControllerBase
    {
        protected DriverRoutesController(ICommandDispatcher commandDispatcher) : base(commandDispatcher)
        {
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateDriverRoute command)
        {
            await DispatchAsync(command);

            return NoContent();
        }

        [Authorize]
        [HttpDelete("name")] // ściezka wywołania ma indentyfikować jednoznacznie usuwany zasób
        public async Task<IActionResult> Delete(string name)
        {
            var command = new DeleteDriverRoute
            {
                Name = name
            };
            await DispatchAsync(command);

            return NoContent();
        }
    }
}