using System;
using System.Threading.Tasks;
using AutoMapper;
using Passenger.Core.Domain;
using Passenger.Core.Repositories;
using Passenger.Infrastructure.DTO;

namespace Passenger.Infrastructure.Services
{
    public class DriverService : IDriverService
    {
        private readonly IDriverRepository _driverRepository;
        private readonly IMapper _mapper;
        public DriverService(IDriverRepository driverRepository, IMapper mapper)
        {
            _driverRepository = driverRepository;
            _mapper = mapper;
        }
        public async Task<DriverDto> GetAsync(Guid userId)
        {
            var driver = await _driverRepository.GetAsync(userId);
            return _mapper.Map<Driver,DriverDto>(driver);
        }

        public async Task RegisterAsync(Guid userId, string vehicleBrand, string vehicleName, int vehicleSeats)
        {
            var driver = GetAsync(userId);
            if(driver != null)
            {
                throw new InvalidOperationException($"Driver already exists for user: '{userId}'.");
            }
            await _driverRepository.AddAsync(new Driver(userId, vehicleBrand, vehicleName, vehicleSeats));
        }
    }
}