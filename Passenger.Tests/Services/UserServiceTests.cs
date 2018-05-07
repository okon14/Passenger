using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;
using Passenger.Core.Repositories;
using Passenger.Infrastructure.Services;
using AutoMapper;
using Passenger.Core.Domain;
using System;

namespace Passenger.Tests.Services
{
    public class UserServiceTests
    {
        [Fact]
        public async Task register_async_should_invoke_add_async_on_repository()
        { 
            var userRepositoryMock = new Mock<IUserRepository>();
            var encrypterMock = new Mock<IEncrypter>();
            var mapperMock = new Mock<IMapper>();

            var userService = new UserService(userRepositoryMock.Object, encrypterMock.Object, mapperMock.Object);  
            await userService.RegisterAsync(Guid.NewGuid(), "user@mail.com", "user", "secret", "user");

            // weryfikacja, że metoda AddAsync dla danego dowolnego użytkownika, została wywołana dokładnie 1x
            userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once); 
        }

    }
}