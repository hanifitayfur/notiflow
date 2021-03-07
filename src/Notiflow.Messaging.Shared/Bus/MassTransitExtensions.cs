using System;
using MassTransit;
using MassTransit.Context;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Notiflow.Messaging.Shared.Bus
{
    public static class MassTransitExtensions
    {
        public static void AddMassTransitBus(this IServiceCollection services, RabbitMqSettings rabbitMqSettings,
            Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> registrationAction = null)
        {
            var serviceProvider = services.BuildServiceProvider();

            // Handle Logging
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            LogContext.ConfigureCurrentLogContext(loggerFactory);

            // Configure Bus
            var bus = ConfigureBus(rabbitMqSettings, registrationAction);

            // Register Interfaces
            services.AddSingleton<IPublishEndpoint>(bus);
            services.AddSingleton<ISendEndpointProvider>(bus);
            services.AddSingleton<IBus>(bus);
            services.AddSingleton<IBusControl>(bus);

            try
            {
                bus.Start();
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger(nameof(AddMassTransitBus));
                logger?.LogError($"Bus could not started. Error: {ex.Message}, StackTrace: {ex.StackTrace}");
            }
        }

        private static IBusControl ConfigureBus(
            RabbitMqSettings settings,
            Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> registrationAction = null)
        {
            if (settings.UseInMemoryQueue)
            {
                return MassTransit.Bus.Factory.CreateUsingInMemory(cfg => { });
            }
            else
            {
                return MassTransit.Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var host = cfg.Host(new Uri(settings.RabbitMqUri), hst =>
                    {
                        hst.Username(settings.UserName);
                        hst.Password(settings.Password);
                    });

                    registrationAction?.Invoke(cfg, host);
                });
            }
        }
    }
}