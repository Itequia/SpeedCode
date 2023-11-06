using Itequia.SpeedCode.Messaging.Configuration;
using Itequia.SpeedCode.Messaging.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System;

namespace Itequia.SpeedCode.Messaging.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddRabbitMQService(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            
            services.AddScoped<IMessageService, RabbitMQ.RabbitMQService>();
            return services;
        }


        public static IServiceCollection AddRabbitMQConsumeService(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddScoped<IConsumerService, RabbitMQ.RabbitMQConsumer>();
            return services;
        }
    }
}
