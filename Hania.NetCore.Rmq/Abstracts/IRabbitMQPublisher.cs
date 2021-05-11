using Hania.NetCore.RabbitMQ.Actions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hania.NetCore.RabbitMQ.Abstracts
{
   public interface IRabbitMQPublisher
    {
        void Publish<T>(T t, Func<T,PublisherOptions> option);
        void Publish<T>(T t, PublisherOptions option);
    }
}
