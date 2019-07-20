namespace Kube.AuditListener.Settings
{
    public class AuditListenerDatabaseSettings : IAuditListenerDatabaseSettings
    {
        public string CollectionName { get; set; }

        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
    }
}
