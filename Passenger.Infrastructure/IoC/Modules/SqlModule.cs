using System.Reflection;
using Autofac;
using Passenger.Infrastructure.EF;

namespace Passenger.Infrastructure.IoC.Modules
{
    public class SqlModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
              // skanujmy nasze assemby RepositoryModule w poszukiwaniu typów
            var assembly = typeof(SqlModule)
                .GetTypeInfo()
                .Assembly;

            // Autofac skanuje nasze Assembly w poszukiwaniu ...
            builder.RegisterAssemblyTypes(assembly)
                   .Where(x => x.IsAssignableTo<ISqlRepository>())
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

        }
        
    }
}