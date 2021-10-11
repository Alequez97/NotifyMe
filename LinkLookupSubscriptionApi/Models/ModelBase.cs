using MongoDB.Bson.Serialization.Attributes;
using System;

namespace LinkLookupSubscriptionApi.Models
{
    public class ModelBase
    {
        [BsonId]
        public Guid Id { get; set; }
    }
}
