using System;

namespace Kube.Infrastructure.RabbitMQ
{
    public interface IMQAgent : IDisposable
    {
        void Subscribe();
    }
}
