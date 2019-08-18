using Kube.Domain.Entities;
using Kube.Persistance.Infrastructure;

namespace Kube.Persistance
{
    public interface IMongoDbRepository
    {
        IDocumentCollection<AuditMessage> Messages { get; }
    }
}