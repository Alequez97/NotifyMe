using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LinkLookupSubscriptionApi.Models
{
    public class Group : ModelBase
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Name { get; set; }

        public List<string> Links { get; set; }
    }
}
