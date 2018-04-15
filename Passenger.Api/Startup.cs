using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Passenger.Infrastructure.Services;
using Passenger.Infrastructure.Repositories;
using Passenger.Core.Repositories;
using Passenger.Infrastructure.Mappers;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Passenger.Infrastructure.IoC.Modules;

namespace Passenger.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IContainer ApplicationContainer { get; private set; } // trzyma konfigurację naszej aplikacji, jakimi klasami implementować nasze interfejsy

        /* //Oryginalna wersja konstrukortora 
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        } */

        // Kostruktor wg. Piotra Gankiewicza
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IUserService,UserService>();
            services.AddScoped<IUserRepository,InMemoryUserRepository>();
            services.AddSingleton(AutoMapperConfig.Initialize());
            services.AddMvc();

            // Implementacja Autofac'a (IoC) dependency injection
            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterModule<CommandModule>();
            builder.RegisterModule(new SettingsModule(Configuration)); // róznica między tym powyższym CommandModule a tym wpisem jest taka, ze wyżej nie potrzebowaliśmy przekazywać parametru do klasy
            ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            // Jeżeli aplikacja się zatrzyma to wywołaj metodę Register i wywołaj na naszym kontenerze Dispose aby wyczyścić nieużytki
            appLifetime.ApplicationStopped.Register(() => this.ApplicationContainer.Dispose());
        }
    }
}
