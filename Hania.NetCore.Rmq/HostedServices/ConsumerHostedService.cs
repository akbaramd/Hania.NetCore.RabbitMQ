using Hania.NetCore.RabbitMQ.Abstracts;
using Hania.NetCore.RabbitMQ.Actions;
using Hania.NetCore.RabbitMQ.Attributes;
using Hania.NetCore.RabbitMQ.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Hania.NetCore.RabbitMQ.HostedServices
{
    public class ConsumerHostedService : IHostedService
    {
        IRabbitMQConsumer _rabbitMQConsumer;
        IServiceProvider _serviceProvider;

        public ConsumerHostedService(IRabbitMQConsumer rabbitMQConsumer, IServiceProvider serviceProvider)
        {
            _rabbitMQConsumer = rabbitMQConsumer;
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var items = TypeHelper.GetAllTypesImplementingOpenGenericType(typeof(IConsumer<>), AppDomain.CurrentDomain.GetAssemblies().SelectMany(x=>x.GetTypes()));

            using (var scope = _serviceProvider.CreateScope())
            {
                foreach (var item in items)
                {
                    var genericResponseTtpe = item.GetInterfaces().First().GetGenericArguments().First();

                    var method = _rabbitMQConsumer.GetType().GetMethod("Consume").MakeGenericMethod(new Type[] { genericResponseTtpe });
                    Func<ConsumerOptions> option = () =>
                    {

                        var attr = item.GetCustomAttribute<ConsumerAttribute>();

                        return new ConsumerOptions
                        {
                            Durable = attr.Durable,
                            AutoDelete = attr.AutoDelete,
                            Exchange = attr.Exchange,
                            Queue = attr.Queue,
                            ExchangeType = attr.ExchangeType,
                            BindingKey = attr.BindingKey,

                        };
                    };
                    var serviceFind = scope.ServiceProvider.GetRequiredService(item);


                    Action<object> action = (data) =>
                    {
                        item.GetMethod("Handle").Invoke(serviceFind, new object[] { data });
                    };

                    method.Invoke(_rabbitMQConsumer, new object[] {
                    option,
                    action
                });
                }
            }

            
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
