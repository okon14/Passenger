using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Passenger.Infrastructure.Services
{
    public class DataInitializer : IDataInitilizer
    {
        private readonly IUserService _userService;
        private readonly IDriverService _driverService;
        private readonly IDriverRouteService _driverRouteService;
        private readonly ILogger<DataInitializer> _logger; 
        public DataInitializer(IUserService userService, 
            IDriverService driverService, 
            IDriverRouteService driverRouteService,
            ILogger<DataInitializer> logger)
        {
            _userService = userService;
            _driverService = driverService;
            _driverRouteService = driverRouteService;
            _logger = logger;
        }
        public async Task SeedAync()
        {
            _logger.LogTrace("Initializing data...");
            // sposób obsługi tasków w pętli
            var tasks = new List<Task>();
            // inicjalizowanie użytkowników z rolą "user"
            for(var i=1; i<=10; i++)
            {
                var userId = Guid.NewGuid();
                var userName = $"user{i}";
                _logger.LogTrace($"Created a new user: {userName}");
                tasks.Add(_userService.RegisterAsync(userId, $"{userName}@mail.com",
                    userName, "secret", "user"));
                tasks.Add(_driverService.CreateAsync(userId));
                tasks.Add(_driverService.SetVehicleAsync(userId, "BMW", "i8"));
                _logger.LogTrace($"Created a new driver for: {userName}");

                tasks.Add(_driverRouteService.AddAsync(userId, "Default route", 1,1,2,2));
                tasks.Add(_driverRouteService.AddAsync(userId, "Job route", 3,4,7,8));
                _logger.LogTrace($"Adding route for: {userName}");
                
            }
            // inicjalizowanie użytkowników z rolą "admin"
            for(var i=1; i<=3; i++)
            {
                var userId = Guid.NewGuid();
                var userName = $"admin{i}";
                _logger.LogTrace($"Created a new user: {userName}");
                tasks.Add(_userService.RegisterAsync(userId, $"{userName}@mail.com",
                    userName, "secret", "admin"));
            }
            await Task.WhenAll(tasks); // odpal wszystkie zadania asynchronicznie i jak się zkończą to lecimy dalej - kolejność wykonania jest przypadkowa, ale wykonanie optymalne
            var counter = await _userService.GetCountAsync();
            _logger.LogTrace($"Summup elements counter: {counter}");
            _logger.LogTrace("Data was initialized.");
        }
    }
}