using System.Threading.Tasks;
using Xunit;
using Newtonsoft.Json;
using Passenger.Infrastructure.DTO;
using FluentAssertions;
using System.Net;
using Passenger.Infrastructure.Commands.Users;

namespace Passenger.Tests.EndToEnd.Controllers
{
    public class UsersControllerTests : ControllerTestsBase
    {
        [Fact]
        public async Task given_valid_email_user_should_exists()
        {
            // Act
            var email = "user1@mail.com";
            var user = await GetUserAsync(email);
            user.Email.Should().BeEquivalentTo(email);
        }

        [Fact]
        public async Task given_valid_email_user_should_not_exists()
        {
            // Act
            var email = "user1000@mail.com";
            var response = await Client.GetAsync($"/users/{email}");
            // sprawdzamy czy odpowiedź z serwera jest 404 - podana strona nie istnieje ??
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task given_unique_email_user_should_be_created()
        {
            // Act
            // Anonimowy obiekt symulujący użytkownika
            // var command = new {};
            // albo typowany
            var command = new CreateUser
            {
                Email = "test@email.com",
                Username = "test",
                Password = "secret"

            };
            var payload = GetPayload(command);
            var response = await Client.PostAsync("users",payload);
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Created);
            response.Headers.Location.ToString().Should().BeEquivalentTo($"user/{command.Email}");

            // sprawdzamy czy użytkownik, rzeczywiście istnieje
            var user = await GetUserAsync(command.Email);
            user.Email.Should().BeEquivalentTo(command.Email);
        }

        private async Task<UserDto> GetUserAsync(string email)
        {
            var response = await Client.GetAsync($"/users/{email}");
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserDto>(responseString);
        }

    }
}