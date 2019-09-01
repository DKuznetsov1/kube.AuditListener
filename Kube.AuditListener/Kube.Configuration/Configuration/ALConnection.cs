namespace Kube.Configuration.Configuration
{
    public class ALQueueConnection
    {
        public string HostName { get; set; }

        public int Port { get; set; }

        public Audit Audit { get; set; }

    }

    public class ALLogging
    {
        public string ConnectionString { get; set; }

        public string Db { get; set; }

        public string Measurment { get; set; }
    }


    public class ALDatabaseConnection
    {
        public string ConnectionString { get; set; }
    }

    public class Audit
    {
        public string Exchange { get; set; }

        public string RoutingKey { get; set; }

    }
}
