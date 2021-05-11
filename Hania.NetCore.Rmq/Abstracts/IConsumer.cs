using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hania.NetCore.RabbitMQ.Abstracts
{
    public interface IConsumer <T>
    {
        Task Handle(T t);
    }
}
