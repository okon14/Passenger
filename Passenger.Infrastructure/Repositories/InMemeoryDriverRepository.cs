using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Passenger.Core.Domain;
using Passenger.Core.Repositories;

namespace Passenger.Infrastructure.Repositories
{
    public class InMemeoryDriverRepository : IDriverRepository
    {
        private static ISet<Driver> _drivers = new HashSet<Driver>();
        public async Task AddAsync(Driver driver)
        {
            _drivers.Add(driver);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Driver>> GetAllAsync()
            => await Task.FromResult(_drivers);

        public async Task<Driver> GetAsync(Guid userId)
           => await Task.FromResult( _drivers.SingleOrDefault(x => x.UserId == userId) );

        public async Task RemoveAsync(Guid id)
        {
            var driver = await GetAsync(id);
            _drivers.Remove(driver);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Driver driver)
        {
            _drivers.Remove(driver);
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(Driver driver)
        {
            await Task.CompletedTask;
        }
    }
}