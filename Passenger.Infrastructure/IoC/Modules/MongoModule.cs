using System.Reflection;
using Autofac;
using MongoDB.Driver;
using Passenger.Infrastructure.Mongo;
using Passenger.Infrastructure.Repositories;

namespace Passenger.Infrastructure.IoC.Modules
{
    public class MongoModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // rejestrownaie sterownika do bazy Mongo
            builder.Register((c,p) =>
            {
                var settings = c.Resolve<MongoSettings>();
                return new MongoClient(settings.ConnectionString);
            }).SingleInstance();

            // rejestrowanie z połączenia z wybraną bazą
            builder.Register((c,p) =>
            {
                var client = c.Resolve<MongoClient>();
                var settings = c.Resolve<MongoSettings>();
                var database = client.GetDatabase(settings.Database);
                return database;
            }).As<IMongoDatabase>();

            // skanujmy nasze assemby RepositoryModule w poszukiwaniu typów
            var assembly = typeof(RepositoryModule)
                .GetTypeInfo()
                .Assembly;

            // Autofac skanuje nasze Assembly w poszukiwaniu ...
            builder.RegisterAssemblyTypes(assembly)
                   .Where(x => x.IsAssignableTo<IMongoRepository>())
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

        }
        
    }
}