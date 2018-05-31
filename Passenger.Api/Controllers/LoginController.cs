using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Passenger.Infrastructure.Extensions;
using Passenger.Infrastructure.Commands;
using Passenger.Infrastructure.Commands.Users;

namespace Passenger.Api.Controllers
{
    public class LoginController : ApiControllerBase
    {
        private readonly IMemoryCache _cache;
        public LoginController(ICommandDispatcher commandDispatcher,
            IMemoryCache cache) : base(commandDispatcher)
        {
            _cache = cache;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Login command)
        {
            command.TokenId = Guid.NewGuid(); // tworzenie IdTokena
            await DispatchAsync(command);
            var jwt = _cache.GetJwt(command.TokenId); // pobranie stworzonego tokena o id zgodnym z powyższym

            return Json(jwt);
        }
    }
}