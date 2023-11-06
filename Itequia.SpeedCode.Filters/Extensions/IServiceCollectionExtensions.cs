using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Itequia.SpeedCode.Filters.Extensions
{
    public enum TextSearchOption { 
        Default,
        ForceCaseInsensitive,
        ForceRemoveSpecialCharacters
    }
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddFilterService(this IServiceCollection services, TextSearchOption options = TextSearchOption.Default)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            OptionsServiceCollectionExtensions.AddOptions(services);
            ServiceCollectionDescriptorExtensions.TryAdd(services, ServiceDescriptor.Scoped<IFilterService>(s => new FilterService(options)));

            return services;
        }
    }
}
