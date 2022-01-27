using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PotatoCounter.Interfaces;

namespace PotatoCounter.Models
{
    public class RabbitMqConfiguration :IRabbitMqConfiguration
    {
        public string Hostname { get; set; }

        public string QueueName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}