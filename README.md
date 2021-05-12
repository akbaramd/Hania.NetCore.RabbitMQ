https://github.com/Akbarsafari00/Hania.NetCore.RabbitMQ/blob/3c8a65ce19003ef4108863bbd91c0b240fada661/Hania.NetCore.Rmq.Sample/Startup.cs#L1-L59

# Hania.NetCore.RabbitMQ

[![Build status](https://ci.appveyor.com/api/projects/status/q261l3sbokafmx1o/branch/master?svg=true)](https://www.nuget.org/packages/Hania.AutoIncluder/)
[![NuGet](http://img.shields.io/nuget/v/Hania.NetCore.RabbitMQ.svg)](https://www.nuget.org/packages/Hania.NetCore.RabbitMQ/)
[![Author](https://img.shields.io/badge/Author-Akbar%20Ahmadi%20Saray-brightgreen.svg)](https://www.nuget.org/packages/Hania.NetCore.RabbitMQ/)
[![Linkdin](https://img.shields.io/badge/Linkdin-Akbar%20Ahmadi%20Saray-orange.svg)](https://www.linkedin.com/in/akbar-ahmadi-saray-5a5b9016b/)


### What is Hania.NetCore.RabbitMQ?

Reactive RabbitMQ Library To improve and make better use of RabbitMQ Message-Broker in .Net Core 3.1 or above

### Where can I get it?

First, [install NuGet](http://docs.nuget.org/docs/start-here/installing-nuget). Then, install [Hania.NetCore.RabbitMQ](https://www.nuget.org/packages/Hania.NetCore.RabbitMQ/) from the package manager console:

```
PM> Install-Package Hania.NetCore.RabbitMQ
```


## How do I get started?
RabbitMQ consists of two parts : Producers and Consumers
### Producer
RabbitMQ Producer writes AMQP messages to a single RabbitMQ queue.

When you configure the destination, you specify the information needed to connect to RabbitMQ client. You also define a queue and the bindings to use. You can use multiple bindings to write messages to one or more exchanges. You can also configure SSL/TLS properties, including default transport protocols and cipher suites. You can optionally configure AMQP message properties.

### Consumer
RabbitMQ is a messaging broker. It accepts messages from publishers, routes them and, if there were queues to route to, stores them for consumption or immediately delivers to consumers, if any.

Consumers consume from queues. In order to consume messages there has to be a queue. When a new consumer is added, assuming there are already messages ready in the queue, deliveries will start immediately.

### Initialize RMQ Client
1 - First of all you need to add `RabbitMqSetting` section to `appsettings.json` file:

```json
{
  "RabbitMqSetting": {
    "Host": "amqp://guest:guest@rabbitmq:5672",
    "Queue": "test",
    "Exchange": "test",
    "ExchangeType": "fanout",
    "Durable": true,
    "AutoDelete": false
  }
```
The only **Required** field to set up the client is **Host** field. The other fields are intended for quantifying client defaults

- Host : RabbitMQ Server URI Address
- Queue : Default Queue Name
- Exchange : Default Exchange Name
- ExchangeType :  Default Exchange Type
- Durable : Default Durable (true)
- AutoDelete : Default AutoDelete (false)



2 - Next we need to add our library to Net Core services
```csharp
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
        
    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHaniaRabbitMQ(Configuratio);
        
        // or you can set spesific type to get this type assembly
        // exmple: 
        // services.AddHaniaRabbitMQ(Configuration,typeof(TestConsumer));
    }
}
```

------------

### Consumer
You Should Use the `Consumer` Attribute to configure the Consumer
And Use `IConsumer<T>` Interface for implementation. (`T` is a Response Model Type)

And you should write you logic in `Handle` method.

**Important:**
This library automatically identifies all Consumers and adds them to the project as a service, and there is no need to define Consumers anywhere. You can also easily use Dependencies Injection in your Consumers. 

```csharp
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
    [Consumer(Exchange = "TestDirectExchange", Queue = "TestDirectQueue", ExchangeType = ExchangeType.Direct ,AutoDelete =true)]
    public class TestConsumer : IConsumer<TestModel>
    {

        ILogger _logger;

        public TestConsumer(ILogger logger)
        {
            _logger = logger;
        }

        public async Task Handle(TestModel model)
        {
            _logger.Log(LogLevel.Trace,$"Received :  Name is {model.FullName}");
        }
    }
}


```

### Producer
Producers in RabbitMq sends event through the  client
#### so , let's publish message :
To test, we write the logic for publishing the message inside the TestController :

**important:**
To send a message, you must inject the `IRabbitMQPublisher` service into your controller.
With `IRabbitMQPublisher` you can easily send messages to your desired Exchenger and Queue. 

```csharp
using Hania.NetCore.RabbitMQ.Abstracts;
using Hania.NetCore.RabbitMQ.Actions;
using Hania.NetCore.RabbitMQ.Sample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;


namespace Hania.NetCore.RabbitMQ.Sample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
      
        private readonly ILogger<TestController> _logger;
        private readonly IRabbitMQPublisher _rabbitMQPublisher;

        public TestController(ILogger<TestController> logger, IRabbitMQPublisher rabbitMQPublisher)
        {
            _logger = logger;
            _rabbitMQPublisher = rabbitMQPublisher;
        }

        [HttpPost]
        public ActionResult Post()
        {
            var options = new PublisherOptions
                {
                AutoDelete = true,
                    Exchange = "TestDirectExchange",
                    Queue = "TestDirectQueue",
                    ExchangeType = ExchangeType.Direct
                };
            _rabbitMQPublisher.Publish(new TestModel("akbar ahmadi saray"),options);
            return Ok();
        }
    }
}

```


You Can Use this library in your netcore project and
Enjoy It :))))

Follow me on Social Media : 

Linkdin : https://www.linkedin.com/in/akbar-ahmadi-saray-5a5b9016b/
Instagram : https://www.instagram.com/_akbarahmadi_


