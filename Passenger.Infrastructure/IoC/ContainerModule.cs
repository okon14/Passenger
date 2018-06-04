using Autofac;
using Microsoft.Extensions.Configuration;
using Passenger.Infrastructure.IoC.Modules;
using Passenger.Infrastructure.Mappers;

namespace Passenger.Infrastructure.IoC
{
    // Klasa służy łatwiejszemu zarządzaniu modułami w Startup.cs w Passenger.Api
    public class ContainerModule : Autofac.Module
    {
        private readonly IConfiguration _configuration;
        public ContainerModule(IConfiguration configuration)
        {   
            _configuration = configuration;      
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(AutoMapperConfig.Initialize())
                .SingleInstance();
            // Ustawiamy co chcemy zaejestrować
            builder.RegisterModule<CommandModule>();
            builder.RegisterModule<RepositoryModule>();
            builder.RegisterModule<MongoModule>();  // obcenie przy tej kolejności dla IUserRepository, repo w pamięci będzie nadpisane przez repo Mongo
            builder.RegisterModule<ServiceModule>();
            builder.RegisterModule(new SettingsModule(_configuration)); // róznica między tym powyższym CommandModule a tym wpisem jest taka, ze wyżej nie potrzebowaliśmy przekazywać parametru do klasy
        }
        
    }
}