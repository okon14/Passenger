using System.Reflection;
using Autofac;
using Passenger.Infrastructure.Services;
using System.Linq;

namespace Passenger.Infrastructure.IoC.Modules
{
    public class ServiceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // skanujmy nasze assemby ServiceModule w poszukiwaniu typów
            var assembly = typeof(ServiceModule)
                .GetTypeInfo()
                .Assembly;

            // Autofac skanuje nasze Assembly w poszukiwaniu ...
            builder.RegisterAssemblyTypes(assembly)
                   .Where(x => x.IsAssignableTo<IService>())
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

           // rejestrujemy usługę uwierzytelniania
           builder.RegisterType<Encrypter>()
                  .As<IEncrypter>()
                  .SingleInstance();           

        }
        
    }
}