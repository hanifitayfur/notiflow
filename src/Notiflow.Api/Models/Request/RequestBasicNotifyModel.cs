using System.Collections.Generic;

namespace Notiflow.Api.Models.Request
{
    public class RequestBasicNotifyModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public List<string> DeviceIds { get; set; }
    }
}