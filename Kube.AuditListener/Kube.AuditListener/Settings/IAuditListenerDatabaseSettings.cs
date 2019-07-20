using System;
using System.Collections.Generic;
using System.Text;

namespace Kube.AuditListener.Settings
{
    public interface IAuditListenerDatabaseSettings
    {
        string CollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
