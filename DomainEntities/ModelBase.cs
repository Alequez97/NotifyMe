using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DomainEntites
{
    public class ModelBase
    {
        [BsonId]
        public Guid Id { get; set; }
    }
}
