using Hania.NetCore.RabbitMQ.Actions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hania.NetCore.RabbitMQ.Abstracts
{
   public interface IRabbitMQConsumer
    {
        void Consume<T>( Func<ConsumerOptions> option,Action<T> action);
    }
}
