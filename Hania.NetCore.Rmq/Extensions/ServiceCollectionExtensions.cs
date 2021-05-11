using Hania.NetCore.RabbitMQ.Abstracts;
using Hania.NetCore.RabbitMQ.Actions;
using Hania.NetCore.RabbitMQ.Attributes;
using Hania.NetCore.RabbitMQ.Helpers;
using Hania.NetCore.RabbitMQ.HostedServices;
using Hania.NetCore.RabbitMQ.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Hania.NetCore.RabbitMQ.Extensions
{
  public static  class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHaniaRabbitMQ(this IServiceCollection services,IConfiguration Configuration,Assembly assembly)
        {

            services.Configure<RabbitMqSetting>(Configuration.GetSection(nameof(RabbitMqSetting)));
            services.AddSingleton(sp =>
                sp.GetRequiredService<IOptions<RabbitMqSetting>>().Value);

            services.AddSingleton<IRabbitMQBus, RabbuitMQBus>();
            services.AddSingleton<IRabbitMQPublisher, RabbitMQPublisher>();
            services.AddSingleton<IRabbitMQConsumer, RabbitMQConsumer>();

            var items = TypeHelper.GetAllTypesImplementingOpenGenericType(typeof(IConsumer<>), assembly.GetTypes());

            foreach (var item in items)
            {
                services.AddScoped(item);
            }


            services.AddHostedService<ConsumerHostedService>();

            return services;
        }

       
    }

}
