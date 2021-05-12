using Hania.NetCore.RabbitMQ.Abstracts;
using Hania.NetCore.RabbitMQ.Attributes;
using Hania.NetCore.RabbitMQ.Sample.Models;
using Hania.NetCore.RabbitMQ.Settings;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hania.NetCore.RabbitMQ.Sample.Consumers
{
    [Consumer(Exchange = "TestEDirect", Queue = "TestQ", BindingKey ="test*", ExchangeType = ExchangeType.Direct ,AutoDelete =true)]
    public class TestDirectConsumer : IConsumer<TestModel>
    {

        public TestDirectConsumer()
        {
        }   

        public async Task Handle(TestModel model)
        {
            Console.WriteLine($"Received from TestDirectConsumer :  Name is {model.FullName}");
        }
    }
}
