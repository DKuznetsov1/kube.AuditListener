using System;
using MongoDB.Bson;

namespace Kube.Domain.Entities
{
    [Serializable]
    public class AuditMessage
    {
        public ObjectId Id;

        public string OperationType { get; set; }

        public DateTime Date { get; set; }
    }
}
