namespace Notiflow.Messaging.Shared.Bus
{
    public class RabbitMqSettings
    {
        public string RabbitMqUri { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string NotificationServiceQueue { get; } = "notiflow-queue";
        public bool UseInMemoryQueue { get; set; }

    }
}