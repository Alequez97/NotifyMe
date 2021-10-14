using MessageSender.Models;

namespace LinkLookupSubscriptionApi.Models
{
    public class Group : ModelBase
    {
        public string Name { get; set; }

        public NotifyConfig NotifyConfig { get; set; }
    }
}
