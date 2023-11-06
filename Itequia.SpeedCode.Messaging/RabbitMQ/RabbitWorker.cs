using Itequia.SpeedCode.Messaging.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Itequia.SpeedCode.Messaging.RabbitMQ
{
    public class RabbitWorker : BackgroundService
    {
        private IConsumerService _consumerService;
        private IMessageHandler _messageHandler;
        private IServiceProvider _serviceProvider;

        public RabbitWorker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
           
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                _consumerService = scope.ServiceProvider.GetRequiredService<IConsumerService>();
                _messageHandler = scope.ServiceProvider.GetRequiredService<IMessageHandler>();

                await _consumerService.ReadMessages(new MessageReceivedCallback((queueName, message) => {

                    _messageHandler.ProcessMessage(queueName, message);

                }));

                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, stoppingToken);
                }
            }                                    
        }
    }
}
