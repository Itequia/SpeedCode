
using RabbitMQ.Client;

namespace Itequia.SpeedCode.Messaging.Interfaces
{
    public interface IMessageService
    {
        IConnection CreateChannel();
        void SendMessage<T>(string queue, T Message);
    }
}
