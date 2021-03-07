using System;

namespace Notiflow.Api.Infrastructure.Ioc
{
    public static class DependencyManager
    {
        public static void RegisterDependencyManager(IDependencyResolver resolver) => Resolver = resolver;

        public static IDependencyResolver Resolver { get; private set; }
    }

    public interface IDependencyResolver
    {
        T Resolve<T>() where T : class;
    }


    public class AspNetCoreDependencyResolver : IDependencyResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public AspNetCoreDependencyResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T Resolve<T>() where T : class
        {
            return (T) _serviceProvider.GetService(typeof(T));
        }
    }

}