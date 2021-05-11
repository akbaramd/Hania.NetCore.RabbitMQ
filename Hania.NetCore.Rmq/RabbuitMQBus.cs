using Hania.NetCore.RabbitMQ.Abstracts;
using Hania.NetCore.RabbitMQ.Settings;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hania.NetCore.RabbitMQ
{
    public class RabbuitMQBus : IRabbitMQBus
    {
        public IModel channel { get;  set; }


        public RabbuitMQBus(RabbitMqSetting setting)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(setting.Host)
            };
            var connection = factory.CreateConnection();
            channel = connection.CreateModel();
        }


    }
}
