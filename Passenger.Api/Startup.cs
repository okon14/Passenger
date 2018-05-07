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
using Passenger.Infrastructure.IoC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Passenger.Infrastructure.Settings;

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
            AuthenticationSetup(services);
            services.AddMvc();
            //ograniczenie dostępu nie tylko poprzez uwirzytelnienie, ale poprzez spełnienie dodatkowych wymagań - konfiguracja polisy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("admin", p => p.RequireRole("admin"));
            });
            services.AddMemoryCache();

            // Implementacja Autofac'a (IoC) dependency injection
            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterModule(new ContainerModule(Configuration));
            ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        // JWT configuration
        private void AuthenticationSetup(IServiceCollection services)
        {
            //JWT https://stackoverflow.com/questions/45686477/jwt-on-net-core-2-0
            services.AddAuthorization(options =>
                {
                    //ograniczenie dostępu nie tylko poprzez uwirzytelnienie, ale poprzez spełnienie dodatkowych wymagań - konfiguracja polisy
                    options.AddPolicy("admin", p => p.RequireRole("admin"));     
                    options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                        .RequireAuthenticatedUser()
                        .Build();
                }
            );
            services.AddAuthentication(o =>
                {
                    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
            );
            
            var jwtSettings = new JwtSettings()
            {
                // nie wiem czy tego nie załatwia SettingExtension w SettingsModule ???
                Key = Configuration.GetSection("jwt:key").Value,
                Issuer = Configuration.GetSection("jwt:issuer").Value,
                ExpiryMinutes = Int32.Parse(Configuration.GetSection("jwt:expiryMinutes").Value)
            };
           
            var signigKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = jwtSettings.Issuer, //tylko ta aplikacja może generować token
                ValidateAudience = false, //zakładamy, że tokemty są generowane tylko dla domeny  pomijamy = false
                IssuerSigningKey = signigKey // w jaki spsosób nasz klucz jest tworzony - pobierane z settings
            };

            //fragment konfigurracji od piotra mieloszyńskiego
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.ClaimsIssuer = jwtSettings.Issuer;
                options.TokenValidationParameters = tokenValidationParameters;
                options.SaveToken = true;
            });
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

            //inicjalizacja wtępna repozytorium
            var generalSettings = app.ApplicationServices.GetService<GeneralSettings>();
            if(generalSettings.SeedData)
            {
                // popbranie z kontenera naszej klasy inicjalizującej
                var dataInitializer = app.ApplicationServices.GetService<IDataInitilizer>();
                dataInitializer.SeedAync();
            }

            app.UseAuthentication(); //JWT - otrzebne bo bez tego nie działało mi uwierzytelnianie na podstawie polis i roli
            app.UseMvc();

            // Jeżeli aplikacja się zatrzyma to wywołaj metodę Register i wywołaj na naszym kontenerze Dispose aby wyczyścić nieużytki
            appLifetime.ApplicationStopped.Register(() => this.ApplicationContainer.Dispose());
        }
    }
}
