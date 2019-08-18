using System;
using System.Configuration;
using Kube.Domain.Entities;
using Kube.Persistance.Infrastructure;
using MongoDB.Driver;

namespace Kube.Persistance
{
    public class AuditListenerRepository: IMongoDbRepository
    {
        private IMongoDatabase database;

        private Lazy<IDocumentCollection<Message>> _messages;

        public AuditListenerRepository()
        {
            // TODO: remove this
            var connectionString = ConfigurationManager.ConnectionStrings["KubeListenerDb"].ConnectionString;
#if DEBUG
            connectionString = connectionString.Replace("mongo-deployment", "localhost", StringComparison.InvariantCultureIgnoreCase);
#endif
            var connection = new MongoUrlBuilder(connectionString);
            var client = new MongoClient(connectionString);

            this.database = client.GetDatabase(connection.DatabaseName);
            this._messages = new Lazy<IDocumentCollection<Message>>(() => 
                new DocumentCollection<Message>(database.GetCollection<Message>("Message")), true);
        }

        public IDocumentCollection<Message> Messages
        {
            get
            {
                return this._messages.Value;
            }
        }
    }
}
