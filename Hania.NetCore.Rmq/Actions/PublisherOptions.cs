using System;
using System.Collections.Generic;
using System.Text;

namespace Hania.NetCore.RabbitMQ.Actions
{
    public class PublisherOptions
    {
        public string Exchange { get; set; }
        public string ExchangeType { get; set; } = "fanout";
        public string Queue { get; set; }
        public bool? Durable { get; set; } = true;
        public bool? AutoDelete { get; set; } = false;
        public string RoutingKey { get; set; }
        public long? TTL { get; set; } = 30000;
    }
}
