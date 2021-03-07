using MassTransit;
using MassTransit.RabbitMqTransport;
using Notiflow.Api.Consumers;

namespace Notiflow.Api.Infrastructure.Ioc
{
    public class ConsumerRegisterModule
    {
        public static void Register(IRabbitMqReceiveEndpointConfigurator e)
        {
            e.Consumer<SendPushNotificationEventConsumer>();
        }
    }
}