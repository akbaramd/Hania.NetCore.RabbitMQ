using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hania.NetCore.RabbitMQ.Abstracts
{
    public interface IRabbitMQBus
    {
        IModel channel { get;  set; }

    }
}
