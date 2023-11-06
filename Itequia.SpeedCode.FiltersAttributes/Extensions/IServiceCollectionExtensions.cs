using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Itequia.SpeedCode.FiltersAttributes.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddGlobalExceptionFilter(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            OptionsServiceCollectionExtensions.AddOptions(services);
            ServiceCollectionDescriptorExtensions.TryAddScoped<GlobalExceptionFilterAttribute>(services);

            return services;
        }
    }
}
