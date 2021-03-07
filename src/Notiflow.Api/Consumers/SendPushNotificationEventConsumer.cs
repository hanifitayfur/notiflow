using System;
using System.Threading.Tasks;
using MassTransit;
using Notiflow.Api.Infrastructure.Ioc;
using Notiflow.Api.Models.Request;
using Notiflow.Api.Service;
using Notiflow.Messaging.Shared.Contracts;

namespace Notiflow.Api.Consumers
{
    public class SendPushNotificationEventConsumer : IConsumer<SendPushNotificationEvent>
    {
        private readonly Lazy<IFirebaseNotificationService> _firebaseNotificationService =
            new(DependencyManager.Resolver.Resolve<IFirebaseNotificationService>);

        private IFirebaseNotificationService FirebaseNotificationService => _firebaseNotificationService.Value;


        public async Task Consume(ConsumeContext<SendPushNotificationEvent> context)
        {
            SendPushNotificationEvent data = context.Message;
            if (data == null)
            {
                throw new ArgumentNullException("push notificaton event context model is null");
            }

            await FirebaseNotificationService.SendNotify(data.ApplicationToken,
                new RequestNotifyModel
                {
                    Title = data.Title,
                    Body = data.Body,
                    Badge = data.Badge,
                    CustomData = data.CustomData,
                    DeviceIds = data.DeviceIds
                });
        }
    }
}