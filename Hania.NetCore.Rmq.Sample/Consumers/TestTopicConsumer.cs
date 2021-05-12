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
    [Consumer(Exchange = "TestETopic", Queue = "TestQ", BindingKey ="test.*", ExchangeType = ExchangeType.Topic ,AutoDelete =true)]
    public class TestTopicConsumer : IConsumer<TestModel>
    {

        public TestTopicConsumer()
        {
        }   

        public async Task Handle(TestModel model)
        {
            Console.WriteLine($"Received From TestTopicConsumer :  Name is {model.FullName}");
        }
    }
}
