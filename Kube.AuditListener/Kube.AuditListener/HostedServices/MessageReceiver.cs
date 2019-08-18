using Kube.Domain.Entities;
using Kube.Infrastructure.RabbitMQ;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kube.AuditListener.HostedServices
{
    public class MessageReceiver : IHostedService, IDisposable
    {
        private readonly IMQAgent<Message> mQAgent;

        public MessageReceiver(IMQAgent<Message> mQAgent)
        {
            this.mQAgent = mQAgent;
        }

        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Starting Receiving Messages");

            this.mQAgent.Subscribe(message =>
            {
                Console.WriteLine(" [x] {0}", message);

                return Task.CompletedTask;
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
