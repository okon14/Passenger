using System.Threading.Tasks;

namespace Passenger.Infrastructure.Services
{
    public interface IFirebirdService : IService
    {
        Task TestowePolaczenieAsync(); 
    }
}