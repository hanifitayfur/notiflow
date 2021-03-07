using System.Net;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Notiflow.Messaging.Shared.Bus;
using Notiflow.Messaging.Shared.Contracts;
using Sample.App.Models;

namespace Sample.App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotifySendEventController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<NotifySendEventController> _logger;

        public NotifySendEventController(ILogger<NotifySendEventController> logger, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        [Route("sendPushNotificationEventSample")]
        [ProducesResponseType(typeof(object), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.UnprocessableEntity)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(object), (int) HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SendPushNotificationEventSample(SendPushNotificationModel model)
        {
            var pushNotificationEvent = new SendPushNotificationEvent()
            {
                ApplicationToken = model.ApplicationToken,
                Title = model.Title,
                Body = model.Body,
                DeviceIds = model.DeviceIds,
                CustomData = model.CustomData,
                Badge = model.Badge
            };

            await HelperEvent.PublishEvent(_publishEndpoint, pushNotificationEvent, _logger);

            return Ok("Success");
        }
    }
}