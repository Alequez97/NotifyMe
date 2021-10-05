using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkLookupSubscriptionApi.Models
{
    public class ModelBase
    {
        [BsonId]
        public Guid Id { get; set; }
    }
}
