using System.IO;
using Kube.AuditListener.HostedServices;
using Kube.Infrastructure.RabbitMQ;
using Kube.Infrastructure.RabbitMQAgent;
using Kube.Persistance;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Kube.Configuration.Configuration;
using Microsoft.Extensions.Configuration;

namespace Kube.AuditListener
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            IConfiguration configuration = null;

            var builder = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    configuration = config
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddEnvironmentVariables() 
                        .Build();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<IMongoDbRepository, AuditListenerRepository>();
                    services.AddTransient(typeof(IMQAgent<>), typeof(RabbitMQAgent<>));
                    services.AddTransient<IHostedService, MessageReceiver>();

                    services.Configure<ALDatabaseConnection>(configuration.GetSection("Connection:Database"));
                    services.Configure<ALQueueConnection>(configuration.GetSection("Connection:Queue"));
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConsole();   
                });

            await builder.RunConsoleAsync();
        }
    }
}
