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
