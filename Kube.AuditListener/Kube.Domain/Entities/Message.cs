using MongoDB.Bson;
using System;

namespace Kube.Domain.Entities
{
    [Serializable]
    public class AuditMessage
    {
        public string OperationType { get; set; }

        public DateTime Date { get; set; }
    }
}
