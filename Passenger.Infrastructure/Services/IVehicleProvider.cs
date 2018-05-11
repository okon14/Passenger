using System.Collections.Generic;
using System.Threading.Tasks;
using Passenger.Core.Domain;
using Passenger.Infrastructure.DTO;

namespace Passenger.Infrastructure.Services
{
    public interface IVehicleProvider : IService
    {
        Task<IEnumerable<VehicleDto>> BrowseAsyncy();
        Task<VehicleDto> GetAsync(string brand, string name);
    }
}