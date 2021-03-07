using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Notiflow.Api.Extensions;
using Notiflow.Api.Models.Request;
using Notiflow.Api.Models.Response;
using Notiflow.Api.Service;

namespace Notiflow.Api.Controllers
{
    public class NotifyController : ControllerBase
    {
        private readonly ILogger<NotifyController> _logger;
        private readonly IFirebaseNotificationService _firebaseNotificationService;

        public NotifyController(ILogger<NotifyController> logger,
            IFirebaseNotificationService firebaseNotificationService)
        {
            _logger = logger;
            _firebaseNotificationService = firebaseNotificationService;
        }
        

        [HttpPost]
        [Route("sendNotifyDetail")]
        [ProducesResponseType(typeof(ResponseNotifyModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.UnprocessableEntity)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(object), (int) HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SendNotify([FromBody] RequestNotifyModel requestNotifyModel)
        {
            var responseNotifyModel =
                await _firebaseNotificationService.SendNotify(Request.Headers.GetApplicationToken(),
                    requestNotifyModel);
            return Ok(responseNotifyModel);
        }

        [HttpPost]
        [Route("sendNotify")]
        [ProducesResponseType(typeof(ResponseNotifyModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.UnprocessableEntity)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(object), (int) HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SendNotifyBasic([FromBody] RequestBasicNotifyModel requestNotifyModel)
        {
            var responseNotifyModel = await _firebaseNotificationService.SendNotify(
                Request.Headers.GetApplicationToken(),
                new RequestNotifyModel
                {
                    Title = requestNotifyModel.Title,
                    Body = requestNotifyModel.Body,
                    DeviceIds = requestNotifyModel.DeviceIds
                });

            return Ok(responseNotifyModel);
        }
    }
}