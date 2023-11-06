using System;
using System.Collections.Generic;
using System.Text;

namespace Itequia.SpeedCode.Messaging.Configuration
{
    public class RabbitMqConfiguration
    {
        public string HostName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public List<string> Queues { get; set; }

        public int Port { get; set; }
    }
}
