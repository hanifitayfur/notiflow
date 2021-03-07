using Newtonsoft.Json;

namespace Notiflow.Api.Infrastructure.Firebase
{
    public class FirebaseCloudMessagingResult
    {
        [JsonProperty("message_id")] public string MessageId { get; set; }

        [JsonProperty("registration_id")] public string RegistrationId { get; set; }

        public string Error { get; set; }
    }
}