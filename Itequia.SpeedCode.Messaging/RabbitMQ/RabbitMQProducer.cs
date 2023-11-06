using Itequia.SpeedCode.Messaging.Configuration;
using Itequia.SpeedCode.Messaging.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace Itequia.SpeedCode.Messaging.RabbitMQ
{
    public class RabbitMQService : IMessageService
    {
        private ConnectionFactory _factory;
        public readonly RabbitMqConfiguration _configuration;

        public RabbitMQService(IConfiguration configuration)
        {

            try
            {
                _configuration = configuration.GetSection("RabbitMqConfiguration").Get<RabbitMqConfiguration>();
                if (_configuration.HostName.Contains("localhost", System.StringComparison.OrdinalIgnoreCase))
                {
                    _factory = new ConnectionFactory()
                    {
                        HostName = _configuration.HostName
                    };
                }
                else
                {
                    _factory = new ConnectionFactory()
                    {
                        UserName = _configuration.Username,
                        Password = _configuration.Password,
                        HostName = _configuration.HostName,
                        Port = _configuration.Port
                    };
                }

                _factory.DispatchConsumersAsync = true;
            }
            catch(Exception ex)
            {
                throw new Exception(string.Format("Error al intentar conectar con RabbitMQ en: {0}, usuario: {1}, password{2}, puerto: {3}. Detalles => {4}",
                                    _configuration.HostName,
                                    _configuration.Username,
                                    _configuration.Password,
                                    _configuration.Port,
                                    ex.InnerException?.StackTrace));
            }
            
        }

        public IConnection CreateChannel()
        {
            return _factory.CreateConnection();
        }

        public void SendMessage<T>(string queue, T message)
        {                      
            var channel = _factory.CreateConnection();

            using var model = channel.CreateModel();
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            model.BasicPublish(string.Format("{0}Exchange", queue),
                                 string.Empty,
                                 basicProperties: null,
                                 body: body);         
        }        
    }
}
