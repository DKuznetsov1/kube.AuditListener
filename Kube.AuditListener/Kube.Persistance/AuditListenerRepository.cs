using System;
using Kube.Configuration.Configuration;
using Kube.Domain.Entities;
using Kube.Persistance.Infrastructure;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Kube.Persistance
{
    public class AuditListenerRepository: IMongoDbRepository
    {
        private IMongoDatabase database;

        private Lazy<IDocumentCollection<AuditMessage>> _messages;

        public AuditListenerRepository(IOptions<ALDatabaseConnection> config)
        {
            var connectionString = config.Value.ConnectionString;
#if DEBUG
            connectionString = connectionString.Replace("mongo-deployment", "localhost", StringComparison.InvariantCultureIgnoreCase);
#endif
            var connection = new MongoUrlBuilder(connectionString);
            var client = new MongoClient(connectionString);

            this.database = client.GetDatabase(connection.DatabaseName);
            this._messages = new Lazy<IDocumentCollection<AuditMessage>>(() =>
                new DocumentCollection<AuditMessage>(database.GetCollection<AuditMessage>("AuditMessage")), true);
        }

        public IDocumentCollection<AuditMessage> Messages
        {
            get
            {
                return this._messages.Value;
            }
        }
    }
}
