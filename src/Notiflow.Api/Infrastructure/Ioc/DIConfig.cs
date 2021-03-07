using Microsoft.Extensions.DependencyInjection;
using Notiflow.Api.Infrastructure.Data;
using Notiflow.Api.Infrastructure.Firebase;
using Notiflow.Api.Service;

namespace Notiflow.Api.Infrastructure.Ioc
{
    public static class DIConfig
    {
        public static void RegisterDependencies(IServiceCollection services)
        {
            services.AddTransient<IFirebaseNotificationService, FirebaseNotificationService>();
            services.AddTransient<IFirebaseNotificationSender, FirebaseNotificationSender>();
            services.AddTransient<IApplicationDataUnit, ApplicationDataUnit>();

        }
    }
}