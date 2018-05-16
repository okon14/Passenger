using System;
using System.Threading.Tasks;

namespace Passenger.Infrastructure.Services
{
    public class RouteManager : IRouteManager
    {
        private static readonly Random Random = new Random();
        public async Task<string> GetAddressAsync(double latitiude, double longitude)
            => await Task.FromResult($"Sample Address {Random.Next(100)}.");
        public double CalculateDistance(double StartLatitude, double StartLongitude, double endLatitude, double endLongitude)
            =>  Random.Next(500, 10000);

        
    }
}