using System;
using System.Threading.Tasks;

namespace Kube.Infrastructure.RabbitMQ
{
    public interface IMQAgent<TMessage> : IDisposable
        where TMessage: class
    {
        void Subscribe(Func<TMessage, Task> subscription);
    }
}
