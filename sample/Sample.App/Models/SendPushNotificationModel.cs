using System.Collections.Generic;

namespace Sample.App.Models
{
    public class SendPushNotificationModel
    {
        public SendPushNotificationModel()
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