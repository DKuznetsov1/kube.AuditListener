using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Kube.Persistance.Infrastructure
{
    public class DocumentCollection<TDocument> : IDocumentCollection<TDocument>
    {
        private readonly IMongoCollection<TDocument> collection;

        public DocumentCollection(IMongoCollection<TDocument> collection) =>
            this.collection = collection;

        public async Task InsertOneAsync(TDocument doc)
        {
            await this.collection.InsertOneAsync(doc);
        }

        public async Task InsertManyAsync(IEnumerable<TDocument> docs)
        {
            await this.collection.InsertManyAsync(docs);
        }

        public async Task<IEnumerable<TDocument>> FindAsync(
            Expression<Func<TDocument, bool>> predicate, 
            int limit)
        {
            var asyncCursor = await this.collection.FindAsync(
                predicate, 
                new FindOptions<TDocument, TDocument>() { Limit = limit });
            return await asyncCursor.ToListAsync();
        }
    }
}
