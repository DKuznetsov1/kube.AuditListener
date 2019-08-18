using System.Configuration;
using Kube.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Kube.Persistance
{
    public class AuditListenerContext
    {
        private IMongoDatabase database;

        public AuditListenerContext()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["KubeListenerDb"].ConnectionString;
            var connection = new MongoUrlBuilder(connectionString);
            var client = new MongoClient(connectionString);
            database = client.GetDatabase(connection.DatabaseName);
            var coll = database.GetCollection<Message>("Message");

            var messageId = new ObjectId("5d4e7c720fab82f6735b69af");

            var message = coll.Find(x => x.Name == messageId).Limit(5).ToListAsync();
        }
    }
}
