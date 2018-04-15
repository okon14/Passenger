using System;
using System.Threading.Tasks;
using Passenger.Infrastructure.DTO;

namespace Passenger.Infrastructure.Services
{
    public interface IDriverService : IService
    {
        // Interface to develop on my own 
        Task<DriverDto> GetAsync(Guid userId);
        Task RegisterAsync(Guid userId, string brand, string name, int seats);
    }
}