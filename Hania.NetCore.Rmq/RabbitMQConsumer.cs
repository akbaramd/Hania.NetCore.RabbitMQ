using Hania.NetCore.RabbitMQ.Abstracts;
using Hania.NetCore.RabbitMQ.Actions;
using Hania.NetCore.RabbitMQ.Settings;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hania.NetCore.RabbitMQ
{
    public class RabbitMQConsumer : IRabbitMQConsumer
    {
        public IRabbitMQBus _bus;
        public RabbitMqSetting _setting;

        public RabbitMQConsumer(IRabbitMQBus bus, RabbitMqSetting setting)
        {
            _bus = bus;
            _setting = setting;
        }

        public void Consume<T>(Func<ConsumerOptions> option, Action<T> action)
        {


            ConsumerOptions consumerOptions;
            if (option != null)
            {
                var invokeResult = option.Invoke();

                consumerOptions = new ConsumerOptions
                {
                    Exchange = (invokeResult.Exchange != null) ? invokeResult.Exchange : _setting.Exchange,
                    ExchangeType = (invokeResult.ExchangeType != null) ? invokeResult.ExchangeType : _setting.ExchangeType,
                    Queue = (invokeResult.Queue != null) ? invokeResult.Queue : _setting.Queue,
                    AutoDelete = (invokeResult.AutoDelete != null) ? invokeResult.AutoDelete : _setting.AutoDelete,
                    Durable = (invokeResult.Durable != null) ? invokeResult.Durable : _setting.Durable,
                    BindingKey = (invokeResult.BindingKey != null) ? invokeResult.BindingKey : "*",
                };
            }
            else
            {
                consumerOptions = new ConsumerOptions
                {
                    Exchange = _setting.Exchange,
                    ExchangeType = _setting.ExchangeType,
                    Queue = _setting.Queue,
                    AutoDelete = _setting.AutoDelete,
                    Durable = _setting.Durable,
                    BindingKey = "*",
                };
            }
            _bus.channel.ExchangeDeclare(consumerOptions.Exchange, consumerOptions.ExchangeType,consumerOptions.Durable.Value,consumerOptions.AutoDelete.Value,null);
            _bus.channel.QueueDeclare(consumerOptions.Queue,
                durable: consumerOptions.Durable.Value,
                exclusive: consumerOptions.Exclusive.Value,
                autoDelete: consumerOptions.AutoDelete.Value,
                arguments: null);
            _bus.channel.QueueBind(consumerOptions.Queue, consumerOptions.Exchange, consumerOptions.BindingKey);
            _bus.channel.BasicQos(0, 10, false);

            var consumer = new EventingBasicConsumer(_bus.channel);
            consumer.Received += (sender, e) => {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var res = JsonConvert.DeserializeObject<T>(message);
                action.Invoke(res);
            };

            _bus.channel.BasicConsume(consumerOptions.Queue, true, consumer);
            Console.WriteLine($"Consumer {typeof(T)} Started ");
        }
    }
}
