using Kube.Domain.Entities;
using Kube.Infrastructure.RabbitMQ;
using Kube.Persistance;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kube.AuditListener.HostedServices
{
    public class MessageReceiver : IHostedService, IDisposable
    {
        private readonly IMQAgent<AuditMessage> mQAgent;
        private readonly IMongoDbRepository context;
        private readonly ILogger<MessageReceiver> logger;

        public MessageReceiver(
            IMQAgent<AuditMessage> mQAgent,
            IMongoDbRepository context,
            ILogger<MessageReceiver> logger)
        {
            this.mQAgent = mQAgent;
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.mQAgent.Subscribe(async message =>
            {
                logger.LogInformation("Message received;{message.OperationType}");

                if (message != null)
                {
                    await context.Messages.InsertOneAsync(message);
                }
            });

            return Task.CompletedTask;
        }

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Stopping.");

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            if (this.mQAgent != null)
            {
                this.mQAgent.Dispose();
            }
        }
    }
}
