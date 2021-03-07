using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json.Linq;
using Notiflow.Api.Infrastructure.Data;
using Notiflow.Api.Infrastructure.Firebase;
using Notiflow.Api.Models.Request;
using Notiflow.Api.Models.Response;

namespace Notiflow.Api.Service
{
    public interface IFirebaseNotificationService
    {
        Task<ResponseNotifyModel> SendNotify(string applicationToken,
            RequestNotifyModel requestModel);
    }

    public class FirebaseNotificationService : IFirebaseNotificationService
    {
        private readonly IFirebaseNotificationSender _firebaseNotificationSender;
        private readonly IApplicationDataUnit _applicationDataUnit;
        private readonly IMapper _mapper;

        public FirebaseNotificationService(IFirebaseNotificationSender firebaseNotificationSender,
            IApplicationDataUnit applicationDataUnit, IMapper mapper)
        {
            _firebaseNotificationSender = firebaseNotificationSender;
            _applicationDataUnit = applicationDataUnit;
            _mapper = mapper;
        }

        public async Task<ResponseNotifyModel> SendNotify(string applicationToken,
            RequestNotifyModel requestModel)
        {
            var application = _applicationDataUnit.GetApplication(applicationToken);
            if (application == null)
            {
                throw new ArgumentNullException("application is null");
            }

            // { notification = { title = string, body = string, badge = 0, sound = default }, data = { actionType = 4 } }
            var payload = new
            {
                notification = new
                {
                    title = requestModel.Title,
                    body = requestModel.Body,
                    badge = requestModel.Badge,
                    sound = requestModel.Sound
                },
            };

            var jsonObject = JObject.FromObject(payload);
            var customData = new JObject();

            if (requestModel.CustomData != null && requestModel.CustomData.Any())
            {
                foreach (var (key, value) in requestModel.CustomData)
                {
                    var jsonElement = (JsonElement) value;
                    switch (jsonElement.ValueKind)
                    {
                        case JsonValueKind.Number:
                            customData.Add(key, jsonElement.GetInt32());
                            break;

                        case JsonValueKind.String:
                            customData.Add(key, jsonElement.GetString());
                            break;

                        case JsonValueKind.True:
                            customData.Add(key, jsonElement.GetBoolean());
                            break;

                        case JsonValueKind.False:
                            customData.Add(key, jsonElement.GetBoolean());
                            break;
                    }
                }

                jsonObject.Add("data", customData);
            }

            var firebaseResult =
                await _firebaseNotificationSender.SendAsync(application.ServerKey, application.SenderId,
                    requestModel.DeviceIds,
                    jsonObject);

            return _mapper.Map<ResponseNotifyModel>(firebaseResult);
        }
    }
}