using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hania.NetCore.RabbitMQ.Sample.Models
{
    public class TestModel
    {
        public TestModel(string fullName)
        {
            FullName = fullName;
        }

        public string FullName { get; set; }
    }
}
