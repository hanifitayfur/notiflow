using System.Collections.Generic;

namespace Notiflow.Messaging.Shared.Contracts
{
    public class SendPushNotificationEvent
    {
        public SendPushNotificationEvent()
        {
            CustomData = new Dictionary<string, object>();
            DeviceIds = new List<string>();
        }
        
        public string ApplicationToken { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public List<string> DeviceIds { get; set; }
        public int? Badge { get; set; }
        public Dictionary<string, object> CustomData { get; set; }
    }
}