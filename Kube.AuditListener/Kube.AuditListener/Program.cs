using Kube.AuditListener.HostedServices;
using Kube.Infrastructure.RabbitMQ;
using Kube.Infrastructure.RabbitMQAgent;
using Kube.Persistance;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Threading.Tasks;
using Kube.Configuration.Configuration;
using Microsoft.Extensions.Configuration;
using System.IO;

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
                    var configurationSettingsString = "appsettings.json";

#if DEBUG
                    configurationSettingsString = "appsettings.Development.json";
#endif

                    configuration = config
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile(configurationSettingsString, optional: false, reloadOnChange: true)
                        .AddEnvironmentVariables() 
                        .Build();

                    var loggerConfiguration = configuration.GetSection("Logging").Get<ALLogging>();

                    Log.Logger = new LoggerConfiguration()
                        .WriteTo.InfluxDB(loggerConfiguration.Measurment, loggerConfiguration.ConnectionString, loggerConfiguration.Db)
                        .Enrich.WithProperty("loggername", loggerConfiguration.Measurment)
                        .CreateLogger();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<IMongoDbRepository, AuditListenerRepository>();
                    services.AddTransient(typeof(IMQAgent<>), typeof(RabbitMQAgent<>));
                    services.AddTransient<IHostedService, MessageReceiver>();

                    services.Configure<ALDatabaseConnection>(configuration.GetSection("Connection:Database"));
                    services.Configure<ALQueueConnection>(configuration.GetSection("Connection:Queue"));
                })
                .UseSerilog();

            await builder.RunConsoleAsync();
        }
    }
}
