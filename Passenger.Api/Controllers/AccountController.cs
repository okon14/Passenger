using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Passenger.Infrastructure.Commands;
using Passenger.Infrastructure.Commands.Users;
using Passenger.Infrastructure.Services;

namespace Passenger.Api.Controllers
{
    public class AccountController : ApiControllerBase
    {
        private readonly IJwtHandler _jwtHandler;
        public AccountController(ICommandDispatcher commandDispatcher,
        IJwtHandler jwtHandler) : base(commandDispatcher)
        {     
            _jwtHandler = jwtHandler;      
        }

        /*[HttpGet]
        [Route("token")]
        public IActionResult Get()
        {
            var token = _jwtHandler.CreateToken("user1@mail.com","admin");
            return Json(token);
        }*/ 

        [HttpGet]
        [Authorize] // tylko zautoryzowany uzytkownik dostanie się do zasobu
        [Route("auth")]
        public IActionResult GetAuth()
        {
            return Content("Auth");
        }
        
        [HttpPut]
        [Route("password")]
        public async Task<IActionResult> Put([FromBody]ChangeUserPassword command)
        {
            await DispatchAsync(command);
            return NoContent();
        }  
    }
}