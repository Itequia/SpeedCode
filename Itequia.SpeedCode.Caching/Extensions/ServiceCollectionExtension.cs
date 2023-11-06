
using Itequia.SpeedCode.Caching.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Itequia.SpeedCode.Caching.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCacheInMemoryService(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            OptionsServiceCollectionExtensions.AddOptions(services);
            ServiceCollectionDescriptorExtensions.TryAdd(services, ServiceDescriptor.Singleton<IMemoryCache>(s => new MemoryCache(new MemoryCacheOptions())));
            ServiceCollectionDescriptorExtensions.TryAddSingleton<ICacheInMemoryService, CacheInMemoryService>(services);
            return services;
        }
    }
}
