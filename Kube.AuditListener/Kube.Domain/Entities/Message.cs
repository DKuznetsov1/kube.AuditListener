using MongoDB.Bson;
using System;

namespace Kube.Domain.Entities
{
    [Serializable]
    public class Message
    {
        public ObjectId Name { get; set; }
    }
}
