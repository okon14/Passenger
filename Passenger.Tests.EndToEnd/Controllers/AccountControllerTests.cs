using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Passenger.Infrastructure.Commands.Users;
using Xunit;

namespace Passenger.Tests.EndToEnd.Controllers
{
    public class AccountControllerTests : ControllerTestsBase
    {
        [Fact]
        public async Task gien_valid_current_and_new_password_should_be_changed()
        {
            // Act
            // Anonimowy obiekt symulujący użytkownika
            // var command = new {};
            // albo typowany
            var command = new ChangeUserPassword
            {
                CurrentPassword = "secret",
                NewPassword = "secret2"
            };
            var payload = GetPayload(command);
            var response = await Client.PutAsync("account/password",payload);
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NoContent);
        }
        
    }
}