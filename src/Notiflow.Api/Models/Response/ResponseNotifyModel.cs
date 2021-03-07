using System.Collections.Generic;
using Notiflow.Api.Infrastructure.Firebase;

namespace Notiflow.Api.Models.Response
{
    public class ResponseNotifyModel
    {
        public string MulticastId { get; set; }

        public int CanonicalIds { get; set; }

        public int Success { get; set; }

        public int Failure { get; set; }

        public List<FirebaseCloudMessagingResult> Results { get; set; }

        public bool IsSuccess()
        {
            return Success > 0 && Failure == 0;
        }
    }
}