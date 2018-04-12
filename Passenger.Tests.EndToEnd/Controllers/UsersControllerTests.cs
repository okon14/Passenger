using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Passenger.Api;
using Xunit;
using Newtonsoft.Json;
using Passenger.Infrastructure.DTO;
using FluentAssertions;
using System.Net;
using System.Text;
using Passenger.Core.Domain;
using Passenger.Infrastructure.Commands.Users;

namespace Passenger.Tests.EndToEnd.Controllers
{
    public class UsersControllerTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client; // Będzie wykonywać zapytania do naszego servera
        public UsersControllerTests()
        {
            // Tworzy w pełni działające API w pmięci
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            _client = _server.CreateClient();
        }

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
            var response = await _client.GetAsync($"/users/{email}");
            // sprawdzamy czy odpowiedź z serwera jest 404 - podana strona nie istnieje ??
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task given_unique_email_user_should_be_created()
        {
            // Act
            // Anonimowy obiekt symulujący użytkownika
            // var request = new {};
            // albo typowany
            var request = new CreateUser
            {
                Email = "test@email.com",
                Username = "test",
                Password = "secret"

            };
            var payload = GetPayload(request);
            var response = await _client.PostAsync("users",payload);
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Created);
            response.Headers.Location.ToString().Should().BeEquivalentTo($"user/{request.Email}");

            // sprawdzamy czy użytkownik, rzeczywiście istnieje
            var user = await GetUserAsync(request.Email);
            user.Email.Should().BeEquivalentTo(request.Email);
        }

        private async Task<UserDto> GetUserAsync(string email)
        {
            var response = await _client.GetAsync($"/users/{email}");
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserDto>(responseString);
        }

        // Pomocnicza metoda do stworzenia z obiektu jego wersji w jsonie
        private static StringContent GetPayload(object data)
        {
            var json = JsonConvert.SerializeObject(data);
            // Content-Type: "application/json"
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}