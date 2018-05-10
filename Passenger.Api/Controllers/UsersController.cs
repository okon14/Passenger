using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Passenger.Infrastructure.Commands;
using Passenger.Infrastructure.Commands.Users;
using Passenger.Infrastructure.DTO;
using Passenger.Infrastructure.Services;
using Passenger.Infrastructure.Settings;

namespace Passenger.Api.Controllers
{
    public class UsersController : ApiControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService, 
            ICommandDispatcher commandDispatcher,
            GeneralSettings settings) : base(commandDispatcher)
        {
            _userService = userService;
        }
        // GET users/{email}
        //[Authorize] //ogólna autoryzacja 
        //[Authorize(Policy = "admin")] //autoryzacja z polisami
        //[Authorize(Roles = "admin,user")] //role based authorisation 
        [HttpGet("{email}")]
        public async Task<IActionResult> Get(string email)
        {
            var user = await _userService.GetAsync(email);
            if(user == null)
            {
                return NotFound();
            }

            return Json(user);
        }

        [Route("liczba")]
        [HttpGet("")]
        public async Task<IActionResult> Get()
        {

            var listaUserow = await _userService.BrowseAsync();
            if(listaUserow == null)
            {
                return NotFound();
            }

            return Json(listaUserow);

            /*
             int? liczba = await _userService.GetCountAsync();
             if (liczba == null)
             {
                 return NotFound();
             }

             return Json(liczba);
             */
        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]CreateUser command)
        {
            await CommandDispatcher.DispatchAsync(command);
            //Location: users/user1@mail.com
            return Created($"user/{command.Email}",new object());
        }

    }
}
