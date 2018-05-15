using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Passenger.Infrastructure.Commands;

namespace Passenger.Api.Controllers
{
    [Route("[controller]")]
    public abstract class ApiControllerBase : Controller
    {
        private readonly ICommandDispatcher _commandDispatcher;

        protected Guid UserId => User?.Identity?.IsAuthenticated == true ?  // jeżeli user nie jest nullem, i posida Identity
            Guid.Parse(User.Identity.Name) :  // z claimsów mozęmy wyciągnac guida
            Guid.Empty;

        protected ApiControllerBase(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }
        protected async Task DispatchAsync<T>(T command) where T : ICommand
        {
            if(command is IAuthenticatedCommand authenticatedCommand)  // feature z C# 7 do rzutowania
            {
                authenticatedCommand.UserId = UserId; // przypisuje identyfikator użytkownika aktualnie zalogowanego w systemie
            }
            await _commandDispatcher.DispatchAsync(command);
        }
        
    }
}