using Microsoft.EntityFrameworkCore;
using Passenger.Core.Domain;
using Passenger.Infrastructure.Settings;

namespace Passenger.Infrastructure.EF
{
    public class PassengerContext : DbContext
    {
        private readonly SqlSettings _sqlSettings;
        public PassengerContext(DbContextOptions<PassengerContext> options, SqlSettings sqlSettings) : base(options)
        {
            _sqlSettings = sqlSettings;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(_sqlSettings.InMemory)
            {
                optionsBuilder.UseInMemoryDatabase(_sqlSettings.ConnectionString);
                return;
            }
            optionsBuilder.UseSqlServer(_sqlSettings.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var userBuilder = modelBuilder.Entity<User>();
            userBuilder.HasKey(x => x.Id);
        }
    }
}