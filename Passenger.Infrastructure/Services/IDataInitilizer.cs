using System.Threading.Tasks;

namespace Passenger.Infrastructure.Services
{
    public interface IDataInitilizer : IService
    {
        // interfejs inicjalizujące dane w API (seedowane nie jest koniczne i można je wyłączyć)
        Task SeedAync();
    }
}