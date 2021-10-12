using MessageSender.Models;
using System.Collections.Generic;

namespace LinkLookupSubscriptionApi.Models.DTO
{
    public class NotifyConfigDto
    {
        public List<string> Links { get; set; }
        public List<string> IgnoreList { get; set; }
        public MessageSender.Models.TelegramConfig TelegramConfig { get; set; }
    }
}
