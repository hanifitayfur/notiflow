using System;
using GreenPipes;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Notiflow.Api.Infrastructure.Firebase;
using Notiflow.Api.Infrastructure.Ioc;
using Notiflow.Messaging.Shared.Bus;

namespace Notiflow.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "DNotify.Api", Version = "v1"});
            });

            services.Configure<FirebaseAppSettings>(Configuration.GetSection("FirebaseAppSettings"));

            services.Configure<RabbitMqSettings>(Configuration.GetSection("RabbitMqSettings"));
            RabbitMqSettings rabbitMqSettings = Configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>();

            services.AddAutoMapper(typeof(Startup));
            
            DIConfig.RegisterDependencies(services);
            var serviceProvider = services.BuildServiceProvider();
            DependencyManager.RegisterDependencyManager(new AspNetCoreDependencyResolver(serviceProvider));

            
            void CreateConsumers(IRabbitMqBusFactoryConfigurator cfg, IRabbitMqHost host)
            {
                cfg.ReceiveEndpoint(rabbitMqSettings.NotificationServiceQueue, e =>
                {
                    e.UseCircuitBreaker(cb =>
                    {
                        cb.TrackingPeriod = TimeSpan.FromMinutes(1);
                        cb.TripThreshold = 15;
                        cb.ActiveThreshold = 10;
                        cb.ResetInterval = TimeSpan.FromMinutes(5);
                    });

                    e.UseRateLimit(1000, TimeSpan.FromSeconds(5));

                    e.UseMessageRetry(r => { r.Incremental(5, new TimeSpan(0, 0, 10), new TimeSpan(0, 0, 10)); });

                    ConsumerRegisterModule.Register(e);
                });
            }
            services.AddMassTransitBus(rabbitMqSettings, CreateConsumers);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DNotify.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}