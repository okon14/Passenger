using System.Threading.Tasks;
using Passenger.Infrastructure.Commands;
using Passenger.Infrastructure.Commands.Drivers;
using Passenger.Infrastructure.Services;

namespace Passenger.Infrastructure.Handlers.Drivers
{
    public class CreateDriverRouteHandler : ICommandHandler<CreateDriverRoute>
    {
        private readonly IDriverRouteService _driverRouteService;
        public CreateDriverRouteHandler(IDriverRouteService driverService)
        {
            _driverRouteService = driverService;
        }
        public async Task HandleAsync(CreateDriverRoute command)
        {
            // dodawanie nowej trasy do drivera
            await _driverRouteService.AddAsync(command.UserId, command.Name,
                command.StartLatitude, command.StartLongitude,
                command.EndLatitude, command.EndtLongitude);
        }
    }
}