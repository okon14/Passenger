using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Passenger.Core.Domain;
using Passenger.Core.Repositories;
using Passenger.Infrastructure.DTO;
using Passenger.Infrastructure.Exceptions;
using Passenger.Infrastructure.Extensions;

namespace Passenger.Infrastructure.Services
{
    public class DriverService : IDriverService
    {
        private readonly IDriverRepository _driverRepository;
        private readonly IUserRepository _userRepository;
        private readonly IVehicleProvider _vehicleProvider;
        private readonly IMapper _mapper;
        public DriverService(IDriverRepository driverRepository, 
            IUserRepository userRepository, 
            IVehicleProvider vehicleProvider,
            IMapper mapper)
        {
            _driverRepository = driverRepository;
            _userRepository = userRepository;
            _vehicleProvider = vehicleProvider;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DriverDto>> BrowseAsync()
        {
            var drivers = await _driverRepository.GetAllAsync();
            //rowiązanie dużo prostsze niż to jakie wypracowałem w UserService.cs
            return _mapper.Map<IEnumerable<DriverDto>>(drivers);
        }

        public async Task CreateAsync(Guid userId)
        {
            var user = await _userRepository.GetOrFailAsync(userId);
            var driver = await _driverRepository.GetAsync(userId);
            if(driver != null)
            {
                throw new ServiceException( Infrastructure.Exceptions.ErrorCodes.DriverAlreadyExists, $"Driver with userId = {userId} already exists."); 
            }
            driver = new Driver(user);
            await _driverRepository.AddAsync(driver);
        }

        public async Task DeleteAsync(Guid userId)
        {
            var driver = await _driverRepository.GetOrFailAsync(userId);
            await _driverRepository.DeleteAsync(driver);
        }

        public async Task<DriverDetailsDto> GetAsync(Guid userId)
        {
            var driver = await _driverRepository.GetAsync(userId);
            return _mapper.Map<DriverDetailsDto>(driver);
        }

        public async Task RegisterAsync(Guid userId, string vehicleBrand, string vehicleName, int vehicleSeats)
        {
            var driver = await _driverRepository.GetAsync(userId);
            if(driver != null)
            {
                throw new ServiceException( Infrastructure.Exceptions.ErrorCodes.DriverAlreadyExists, $"Driver already exists for user: '{userId}'.");
            }
            await _driverRepository.AddAsync(new Driver(userId, vehicleBrand, vehicleName, vehicleSeats));
        }

        public async Task SetVehicleAsync(Guid userId, string brand, string name)
        {
            var driver = await _driverRepository.GetOrFailAsync(userId);
            var vehicleDto = await _vehicleProvider.GetAsync(brand,name);
            var vehicle = Vehicle.Create(brand, name, vehicleDto.Seats);;
            driver.SetVehicle(vehicle);
            await _driverRepository.UpdateAsync(driver);
        }
    }
}