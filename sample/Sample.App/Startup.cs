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
using Notiflow.Messaging.Shared.Bus;

namespace Sample.App
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "DNotify.Sample", Version = "v1"});
            });

            services.Configure<RabbitMqSettings>(Configuration.GetSection("RabbitMqSettings"));
            var rabbitMqSettings = Configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>();

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
                });
            }
            
            services.AddMassTransitBus(rabbitMqSettings, CreateConsumers);
        }

       

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DNotify.Sample v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}