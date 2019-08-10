using Kube.AuditListener.HostedServices;
using Kube.Infrastructure.RabbitMQ;
using Kube.Infrastructure.RabbitMQAgent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Threading.Tasks;

namespace Kube.AuditListener
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = new HostBuilder().ConfigureAppConfiguration((hostingContext, config) => { })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<IMQAgent>(provider => new RabbitMQAgent());
                    services.AddTransient<IHostedService, MessageReceiver>();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConsole();   
                });

            await builder.RunConsoleAsync();
        }
    }
}
