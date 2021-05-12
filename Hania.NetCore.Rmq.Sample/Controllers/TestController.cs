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
        private readonly IRabbitMQBus _rabbitMQBus;

        public TestController(ILogger<TestController> logger, IRabbitMQBus rabbitMQBus)
        {
            _logger = logger;
            _rabbitMQBus = rabbitMQBus;
        }

        [HttpPost]
        public ActionResult Post()
        {
            
            return Ok();
        }

        [HttpGet]
        public ActionResult GET()
        {
            _rabbitMQBus.Publish(new TestModel("akbar ahmadi saray"),"TopicQueue","TestE","test.sayhello",true,true);
            return Ok();
        }
    }
}
