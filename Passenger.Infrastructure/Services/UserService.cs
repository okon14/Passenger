using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Passenger.Core.Domain;
using Passenger.Core.Repositories;
using Passenger.Infrastructure.DTO;

namespace Passenger.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncrypter _encrypter;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IEncrypter encrypter, IMapper mapper)
        {
            _userRepository = userRepository;
            _encrypter = encrypter;
            _mapper = mapper;
        }

        public async Task<UserDto> GetAsync(string email)
        {
            var user = await _userRepository.GetAsync(email);

            return _mapper.Map<User,UserDto>(user);
        }

        public async Task<int> GetCountAsync()
        {
            int counter = await _userRepository.GetCountAsync();

            return counter;
        }

        public async Task<IEnumerable<UserDto>> BrowseAsync()
        {
            IEnumerable<User> listOfUsers = await _userRepository.GetAllAsync();

            List<UserDto> listOfUsersDto = new List<UserDto>();

            foreach(var user in listOfUsers)
            {
                listOfUsersDto.Add(_mapper.Map<User, UserDto>(user));
            }

            return listOfUsersDto;

        }

        public async Task LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetAsync(email);
            if(user == null)
            {
                throw new Exception("Invalid credentials");
            }
            var hash = _encrypter.GetHash(password, user.Salt);
            if(user.Password == hash)
            {
                return; // jak dobre hasło to koniec ;)
            }
            throw new Exception("Invalid credentials");
        }

        public async Task RegisterAsync(Guid userId, string email, string username, string password, string role)
        {
            var user = await _userRepository.GetAsync(email);
            if(user != null)
            {
                throw new Exception($"User with {email} already exists!");
            }

            var salt = _encrypter.GetSalt(password);
            var hash = _encrypter.GetHash(password, salt);
            user = new User(userId, email, username, hash, salt, role);  // zapisanie użytkownia z bezpiecznym hasłem
            await _userRepository.AddAsync(user);
        }
    }
}