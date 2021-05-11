using System;
using System.Collections.Generic;
using System.Text;

namespace Hania.NetCore.RabbitMQ.Settings
{
    public class RabbitMqSetting
    {
        public string Host { get; set; }
        public string Queue { get; set; }
        public string Exchange { get; set; }
        public string ExchangeType { get; set; }
        public bool Durable { get; set; }
        public bool AutoDelete { get; set; }
    }
}
