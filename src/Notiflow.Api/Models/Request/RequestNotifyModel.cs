using System.Collections.Generic;

namespace Notiflow.Api.Models.Request
{
    public class RequestNotifyModel : RequestBasicNotifyModel
    {
        public RequestNotifyModel()
        {
            Sound = "default";
            CustomData = new Dictionary<string, object>();
        }

        public int? Badge { get; set; }

        /// <summary>
        /// default is Sound's default
        /// </summary>
        public string Sound { get; set; }

        public Dictionary<string, object> CustomData { get; set; }
    }
    
    
}