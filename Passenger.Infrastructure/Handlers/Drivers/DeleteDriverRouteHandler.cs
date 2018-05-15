using System.Threading.Tasks;
using Passenger.Infrastructure.Commands;
using Passenger.Infrastructure.Commands.Drivers;
using Passenger.Infrastructure.Services;

namespace Passenger.Infrastructure.Handlers.Drivers
{
    public class DeleteDriverRouteHandler : ICommandHandler<DeleteDriverRoute>
    {
        private readonly IDriverRouteService _driverRouteService;
        public DeleteDriverRouteHandler(IDriverRouteService driverService)
        {
            _driverRouteService = driverService;
        }
        public async Task HandleAsync(DeleteDriverRoute command)
        {
            // dodawanie nowej trasy do drivera
            await _driverRouteService.DeleteAsync(command.UserId, command.Name);
        }
    }
}