using Itequia.SpeedCode.Persistence.Interfaces.Mongo;
using Itequia.SpeedCode.Persistence.Models;
using Itequia.SpeedCode.Persistence.Mongo;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Itequia.SpeedCode.Persistence.Extensions
{
    
    public static class MongoDBExtensions
    {
        public static IServiceCollection AddMongoDBRepository<T>(this IServiceCollection services) where T : MongoBaseEntity
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddScoped<IMongoRepository<T>, MongoRepository<T>>();
            return services;
        }
    }
}
