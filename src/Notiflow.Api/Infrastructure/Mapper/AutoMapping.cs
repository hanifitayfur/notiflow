using AutoMapper;
using Notiflow.Api.Models.Response;
using FirebaseNotificationResponseModel = Notiflow.Api.Infrastructure.Firebase.FirebaseNotificationResponseModel;

namespace Notiflow.Api.Infrastructure.Mapper
{
    public class AutoMapping:Profile
    {
        public AutoMapping()
        {
            CreateMap<FirebaseNotificationResponseModel, ResponseNotifyModel>();
        }
    }
}