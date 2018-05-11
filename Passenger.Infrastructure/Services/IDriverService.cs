using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Passenger.Infrastructure.DTO;

namespace Passenger.Infrastructure.Services
{
    public interface IDriverService : IService
    {
        // Interface to develop on my own 
        Task<DriverDto> GetAsync(Guid userId);
        Task<IEnumerable<DriverDto>> BrowseAsync();
        Task RegisterAsync(Guid userId, string brand, string name, int seats);

        Task CreateAsync(Guid userId);

        Task SetVehicleAsync(Guid userId, string brand, string name);
    }
}