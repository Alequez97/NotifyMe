using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
