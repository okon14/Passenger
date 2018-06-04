using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Passenger.Core.Domain;
using Passenger.Core.Repositories;

namespace Passenger.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository, IMongoRepository
    {
        private readonly IMongoDatabase _database;
        public UserRepository(IMongoDatabase database)
        {
            _database = database;
        }
        public async Task<User> GetAsync(Guid id)
            => await Users.AsQueryable().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<User> GetAsync(string email)
           => await Users.AsQueryable().FirstOrDefaultAsync(x => x.Email == email.ToLowerInvariant());

        public async Task<int> GetCountAsync()
        {
            var counter = await Users.CountAsync(new BsonDocument());
            return (int)counter;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
            => await Users.AsQueryable().ToListAsync<User>();

        public async Task AddAsync(User user)
            => await Users.InsertOneAsync(user);

        public async Task RemoveAsync(Guid id)
            => await Users.DeleteOneAsync(x => x.Id == id);

        public async Task UpdateAsync(User user)
            => await Users.UpdateOneAsync<User>(x => x.Id == user.Id, user.ToBsonDocument());

        // jak dostać użytkowników składowanych w naszej bazie?
        private IMongoCollection<User> Users => _database.GetCollection<User>("Users");
    }
}