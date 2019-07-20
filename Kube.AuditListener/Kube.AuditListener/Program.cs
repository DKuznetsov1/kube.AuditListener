using Kube.Infrastructure.RabbitMQAgent;
using System;

namespace Kube.AuditLIstener
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");
            var mqAgent = new RabbitMQAgent();
            mqAgent.Subscribe();
        }
    }
}
