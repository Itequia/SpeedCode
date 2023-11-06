using Itequia.SpeedCode.Logger.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Itequia.SpeedCode.Logger.Extensions
{   
    public static class LogServiceExtension
    {
        public static IServiceCollection AddLogService(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddScoped<ILogService, LogService>();
            return services;
        }
    }
}
