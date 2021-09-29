using LinkLookupBackgroundService.Configuration.Models;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkLookupSubscriptionApi.Models
{
    public class User
    {
        [BsonId]
        public Guid Id { get; set; }

        public string Username { get; set; }

        public NotifyConfig NotifyConfig { get; set; }

    }
}
