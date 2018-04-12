using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Passenger.Tests.Services
{
    public class UserServiceTests
    {
        [Fact]
        public async Task Test()
        {
            true.Should().BeTrue();
            await Task.CompletedTask;
        }

        int Add(int x, int y)
        {
            return x + y;
        }
    }
}