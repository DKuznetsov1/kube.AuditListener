using Kube.Domain.Entities;
using Kube.Infrastructure.RabbitMQ;
using Kube.Persistance;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kube.AuditListener.HostedServices
{
    public class MessageReceiver : IHostedService, IDisposable
    {
        private readonly IMQAgent<Message> mQAgent;
        private readonly IMongoDbRepository context;

        public MessageReceiver(
            IMQAgent<Message> mQAgent,
            IMongoDbRepository context)
        {
            this.mQAgent = mQAgent;
            this.context = context;
        }

        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Starting Receiving Messages");

            this.mQAgent.Subscribe(async message =>
            {
                Console.WriteLine(" [x] {0}", message);

                if (message != null)
                {
                    await context.Messages.InsertOneAsync(message);
                }
                // TODO: remove this
                else
                {
                    await context.Messages.InsertOneAsync(
                        new Message()
                        {
                            Name = new MongoDB.Bson.ObjectId()
                        });
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
