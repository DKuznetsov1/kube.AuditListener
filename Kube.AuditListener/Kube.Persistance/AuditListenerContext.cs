using MongoDB.Driver;

namespace Kube.Persistance
{
    public class AuditListenerContext
    {
        private IMongoDatabase database;

        public AuditListenerContext()
        {
            var connectionString = "";
            var connection = new MongoUrlBuilder(connectionString);
            var client = new MongoClient(connectionString);
            database = client.GetDatabase(connection.DatabaseName);
        }
    }
}
