using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Passenger.Infrastructure.Commands;
using Passenger.Infrastructure.Commands.Users;
using Passenger.Infrastructure.Extensions;
using Passenger.Infrastructure.Services;

namespace Passenger.Infrastructure.Handlers.Users
{
    public class LoginHandler : ICommandHandler<Login>
    {
        private readonly IUserService _userService;
        private readonly IJwtHandler _jwtHandler;
        private readonly IMemoryCache _cache;

        public LoginHandler(IUserService userService,
            IJwtHandler jwtHandler,
            IMemoryCache cache)
        {
           _userService = userService;
           _jwtHandler = jwtHandler;
           _cache = cache;
        } 
        public async Task HandleAsync(Login command)
        {
            await _userService.LoginAsync(command.Email, command.Password);
            var user = await _userService.GetAsync(command.Email);
            var jwt =_jwtHandler.CreateToken(user.Id,user.Role); // stworzenie tokena
            _cache.SetJwt(command.TokenId, jwt);  //zapisanie tokena w cache - implementacja CQRS - handler nic nie zwraca
        }
    }
}