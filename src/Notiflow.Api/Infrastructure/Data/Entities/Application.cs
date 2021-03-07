namespace Notiflow.Api.Infrastructure.Data.Entities
{
    public class Application : BaseEntity
    {
        public string Token { get; set; }
        public string ServerKey { get; set; }
        public string SenderId { get; set; }
        public string Name { get; set; }
    }
}