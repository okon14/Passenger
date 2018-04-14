using System.Reflection;
using Autofac;
using Passenger.Infrastructure.Commands;

namespace Passenger.Infrastructure.IoC.Modules
{
    public class CommandModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // jakdopasować komendę do command handlera? - refleksja
            var assembly = typeof(CommandModule)
                .GetTypeInfo()
                .Assembly;

            // Autofac skanuje nasze Assembly projektu Infrastructure w poszukiwaniu
            builder.RegisterAssemblyTypes(assembly)
                   .AsClosedTypesOf(typeof(ICommandHandler<>)) // typów domykających, czyli implementacje 
                   .InstancePerLifetimeScope();

            // Ustawiamy co chcemy zaejestrować
            builder.RegisterType<CommandDispatcher>()
                .As<ICommandDispatcher>()
                .InstancePerLifetimeScope(); // cykl życia per żądanie http
        }
    }
}