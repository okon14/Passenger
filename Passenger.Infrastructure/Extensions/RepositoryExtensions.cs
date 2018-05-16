using System;
using System.Threading.Tasks;
using Passenger.Core.Domain;
using Passenger.Core.Repositories;

namespace Passenger.Infrastructure.Extensions
{
    public static class RepositoryExtensions
    {
        public static async Task<Driver> GetOrFailAsync(this IDriverRepository repository, Guid userId )
        {
            var driver = await repository.GetAsync(userId);
            if(driver == null)
            {
                throw new InvalidOperationException($"Driver for user: '{userId}' was not found.");
            }
            return driver;
        }

        public static async Task<User> GetOrFailAsync(this IUserRepository repository, Guid userId )
        {
            var user = await repository.GetAsync(userId);
            if(user == null)
            {
                throw new InvalidOperationException($"User with id: '{userId}' was not found.");
            }
            return user;
        }
    }
}