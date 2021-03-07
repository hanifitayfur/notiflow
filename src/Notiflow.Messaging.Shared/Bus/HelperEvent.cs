using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Notiflow.Messaging.Shared.Bus
{
    public class HelperEvent
    {
        public static async Task PublishEvent<TEvent>(IPublishEndpoint publishEndpoint, TEvent @event, ILogger logger)
            where TEvent : class
        {
            try
            {
                await publishEndpoint.Publish(@event);
                logger.LogInformation("Event Published Successfully.Event \"" + @event.GetType().FullName + ". Event Data: " + JsonConvert.SerializeObject(@event));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error Creating Event \"" + @event.GetType().FullName + ". Error:" + ex.Message + ". Event Data: " + JsonConvert.SerializeObject(@event));
            }
        }
    }
}