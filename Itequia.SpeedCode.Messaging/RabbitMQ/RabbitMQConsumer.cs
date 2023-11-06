using Itequia.SpeedCode.Messaging.Configuration;
using Itequia.SpeedCode.Messaging.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itequia.SpeedCode.Messaging.RabbitMQ
{
    public delegate void MessageReceivedCallback (string queueName, string message);

    public class RabbitMQConsumer: IConsumerService, IDisposable
    {
        public readonly RabbitMqConfiguration _configuration;

        //private readonly IConfiguration _configuration;
        private List<string> queueList = new List<string>();
        private Dictionary<string, IModel> _models = new Dictionary<string, IModel>();
        private Dictionary<string, IConnection> _connections = new Dictionary<string, IConnection>();
        private const string exchangeSufix = "{0}Exchange";

        public RabbitMQConsumer(IConfiguration configuration, IMessageService rabbitMqService)
        {
            try
            {
                _configuration = configuration.GetSection("RabbitMqConfiguration").Get<RabbitMqConfiguration>();
                queueList = _configuration.Queues;

                queueList.ForEach(q => {

                    var _conn = rabbitMqService.CreateChannel();
                    var _model = _conn.CreateModel();
                    _model.QueueDeclare(q, durable: true, exclusive: false, autoDelete: false);
                    _model.ExchangeDeclare(string.Format(exchangeSufix, q), ExchangeType.Fanout, durable: true, autoDelete: false);
                    _model.QueueBind(q, string.Format(exchangeSufix, q), string.Empty);
                    _connections.Add(q, _conn);
                    _models.Add(q, _model);
                });
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
        
        public async Task ReadMessages(MessageReceivedCallback callback)
        {
            _models.ToList().ForEach(_model => {

                var consumer = new AsyncEventingBasicConsumer(_model.Value);
                consumer.Received += async (ch, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var text = System.Text.Encoding.UTF8.GetString(body);
                    Console.WriteLine(text);
                    if (callback != null)
                    {
                        callback(_model.Key, text);
                    }

                    await Task.CompletedTask;
                    _model.Value.BasicAck(ea.DeliveryTag, false);
                };
                _model.Value.BasicConsume(_model.Key, false, consumer);

            });
            
            await Task.CompletedTask;
        }

        public void Dispose()
        {
            _models.ToList().ForEach(_model => {
                if (_model.Value.IsOpen)
                    _model.Value.Close();
            });

            _connections.ToList().ForEach(_conn => {
                if (_conn.Value.IsOpen)
                    _conn.Value.Close();

            });
        }
    }
}
