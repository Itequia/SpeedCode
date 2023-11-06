using Itequia.SpeedCode.Messaging.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Itequia.SpeedCode.Messaging.Interfaces
{
    public interface IConsumerService
    {
        Task ReadMessages(MessageReceivedCallback callback);
    }
}
