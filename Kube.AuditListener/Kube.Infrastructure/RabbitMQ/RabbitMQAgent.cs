using Kube.Infrastructure.RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Kube.Infrastructure.RabbitMQAgent
{
    public class RabbitMQAgent : IMQAgent
    {
        private readonly string queue = "kube.audit.queue";
        private readonly string exchangeName = "kube.audit.exchange";

        private IConnection connection;
        private IModel channel;

        public void Subscribe()
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq-management-deployment", Port = 5674 };
            this.connection = factory.CreateConnection();
            this.channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: exchangeName, type: "topic");
            channel.QueueDeclare(this.queue, true, false, false, null);
            channel.QueueBind(queue: this.queue, exchange: this.exchangeName, routingKey: "*");

            Console.WriteLine(" [*] Waiting for logs.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] {0}", message);
            };
            channel.BasicConsume(queue: this.queue, autoAck: true, consumer: consumer);
            
        }

        public void Dispose()
        {
            channel.Dispose();
            connection.Dispose();
        }
    }
}
