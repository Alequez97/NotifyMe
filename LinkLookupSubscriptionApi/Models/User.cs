using LinkLookupBackgroundService.Configuration.Models;

namespace LinkLookupSubscriptionApi.Models
{
    public class User : ModelBase
    {
        public string Username { get; set; }

        public NotifyConfig NotifyConfig { get; set; }
    }
}
