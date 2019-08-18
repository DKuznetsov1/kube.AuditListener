using Kube.Infrastructure.RabbitMQ;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Kube.Infrastructure.RabbitMQAgent
{
    public class RabbitMQAgent<TMessage> : IMQAgent<TMessage>
        where TMessage: class
    {
        private readonly string queue = "kube.audit.queue";
        private readonly string exchangeName = "kube.audit.exchange";

        private IConnection connection;
        private IModel channel;

        public void Subscribe(Func<TMessage, Task> subscription)
        {
            // TODO: remove this
#if DEBUG
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 5674 };
#else
            var factory = new ConnectionFactory() { HostName = "rabbitmq-management-deployment", Port = 5674 };
#endif
            this.connection = factory.CreateConnection();
            this.channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: exchangeName, type: "topic");
            channel.QueueDeclare(this.queue, true, false, false, null);
            channel.QueueBind(queue: this.queue, exchange: this.exchangeName, routingKey: "*");

            Console.WriteLine(" [*] Waiting for logs.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);

                TMessage messageObject = null;
                try
                {
                    messageObject = JsonConvert.DeserializeObject<TMessage>(message);
                }
                catch { }

                if (subscription != null)
                {
                    await subscription(messageObject);
                }
            };

            channel.BasicConsume(queue: this.queue, autoAck: true, consumer: consumer);
        }

        public void Dispose()
        {
            if (channel != null)
            {
                channel.Dispose();
            }
            
            if (connection != null)
            {
                connection.Dispose();
            }
        }
    }
}
