using Hania.NetCore.RabbitMQ.Abstracts;
using Hania.NetCore.RabbitMQ.Actions;
using Hania.NetCore.RabbitMQ.Settings;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hania.NetCore.RabbitMQ
{
    public class RabbitMQPublisher : IRabbitMQPublisher
    {
        public IRabbitMQBus _bus;
        public RabbitMqSetting _setting;

        public RabbitMQPublisher(IRabbitMQBus bus, RabbitMqSetting setting)
        {
            _bus = bus;
            _setting = setting;
        }



        public void Publish<T>(T t, Func<T,PublisherOptions> option = null)
        {
            
            PublisherOptions publisherOptions;
            if (option != null)
            {
                var invokeResult = option.Invoke(t);
            
                    publisherOptions = new PublisherOptions
                    {
                        Exchange = (invokeResult.Exchange!=null)?invokeResult.Exchange: _setting.Exchange,
                        ExchangeType = (invokeResult.ExchangeType != null) ? invokeResult.ExchangeType : _setting.ExchangeType,
                        Queue = (invokeResult.Queue != null) ? invokeResult.Queue : _setting.Queue,
                        AutoDelete = (invokeResult.AutoDelete != null) ? invokeResult.AutoDelete : _setting.AutoDelete,
                        Durable = (invokeResult.Durable != null) ? invokeResult.Durable : _setting.Durable,
                        RoutingKey = (invokeResult.RoutingKey != null) ? invokeResult.RoutingKey : "*",
                    };
            }
            else
            {
                publisherOptions = new PublisherOptions
                {
                    Exchange = _setting.Exchange,
                    ExchangeType = _setting.ExchangeType,
                    Queue = _setting.Queue,
                    AutoDelete = _setting.AutoDelete,
                    Durable = _setting.Durable,
                    RoutingKey = "*"
                };
            }
            var ttl = new Dictionary<string, object>
            {
                {"x-message-ttl", publisherOptions.TTL}
            };
            _bus.channel.ExchangeDeclare(publisherOptions.Exchange,publisherOptions.ExchangeType,publisherOptions.Durable.Value,publisherOptions.AutoDelete.Value,ttl);

             var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(t));

                _bus.channel.BasicPublish(publisherOptions.Exchange,publisherOptions.RoutingKey , null, body);
        }

        public void Publish<T>(T t, PublisherOptions option)
        {
            Publish<T>(t, x => option);
        }
    }
}
