using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Kube.Persistance.Infrastructure
{
    public interface IDocumentCollection<TDocument>
    {
        Task<IEnumerable<TDocument>> FindAsync(Expression<Func<TDocument, bool>> predicate, int limit);

        Task InsertManyAsync(IEnumerable<TDocument> docs);

        Task InsertOneAsync(TDocument doc);
    }
}