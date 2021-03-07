using System.Collections.Generic;
using Newtonsoft.Json;

namespace Notiflow.Api.Infrastructure.Firebase
{
    public class FirebaseNotificationResponseModel
    {
        [JsonProperty("multicast_id")] public string MulticastId { get; set; }

        [JsonProperty("canonical_ids")] public int CanonicalIds { get; set; }

        public int Success { get; set; }

        public int Failure { get; set; }

        public List<FirebaseCloudMessagingResult> Results { get; set; }

        public bool IsSuccess()
        {
            return Success > 0 && Failure == 0;
        }
    }
}