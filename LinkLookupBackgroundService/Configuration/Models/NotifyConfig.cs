using System.Xml.Serialization;

namespace LinkLookupBackgroundService.Configuration.Models
{
    public sealed class NotifyConfig
    {
        public TelegramConfig TelegramConfig { get; set; }
    }
}
