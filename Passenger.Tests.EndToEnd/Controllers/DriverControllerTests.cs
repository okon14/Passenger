using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Passenger.Infrastructure.Commands.Drivers;
using Passenger.Infrastructure.DTO;
using Xunit;

namespace Passenger.Tests.EndToEnd.Controllers
{
    public class DriverControllerTests : ControllerTestsBase
    {
        [Fact]
        public async Task given_unique_user_id_driver_should_be_created()
        {
            var newDriversUser = new UserDto();
            try
            {
                newDriversUser = await GetUserAsync("user1@mail.com");
            }
            catch (Exception e)
            {
                var komunikat = e;
            }
            // Act
            // Anonimowy obiekt symulujący użytkownika
            // var command = new {};
            // albo typowany
            var command = new CreateDriver
            {
                UserId = newDriversUser.Id,
                VehicleBrand = "Ford",
                VehicleName = "Focus",
                VehicleSeats = 5,
            };
            var payload = GetPayload(command);
            var response = await Client.PostAsync("drivers",payload);
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Created);
            response.Headers.Location.ToString().Should().BeEquivalentTo($"drivers/{command.UserId}");

            // sprawdzamy czy użytkownik, rzeczywiście istnieje
            var driver = await GetDriverAsync(command.UserId);
            var driversUserId = driver.UserId.ToString();
            driversUserId.Should().BeEquivalentTo(command.UserId.ToString());
        }

        private async Task<UserDto> GetUserAsync(string email)
        {
            var response = await Client.GetAsync($"/users/{email}");
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserDto>(responseString);
        }

        private async Task<DriverDto> GetDriverAsync(Guid userId)
        {
            var response = await Client.GetAsync($"/drivers/{userId.ToString()}");
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DriverDto>(responseString);
        }
        
    }
}