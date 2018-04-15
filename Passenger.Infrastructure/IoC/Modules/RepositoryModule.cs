using System.Reflection;
using Autofac;
using Passenger.Core.Repositories;
using System.Linq;

namespace Passenger.Infrastructure.IoC.Modules
{
    public class RepositoryModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // skanujmy nasze assemby RepositoryModule w poszukiwaniu typów
            var assembly = typeof(RepositoryModule)
                .GetTypeInfo()
                .Assembly;

            // Autofac skanuje nasze Assembly w poszukiwaniu ...
            builder.RegisterAssemblyTypes(assembly)
                   .Where(x => x.IsAssignableTo<IRepository>())
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

        }
        
    }
}