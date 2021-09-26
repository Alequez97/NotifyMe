using LinkLookup.Models;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace LinkLookupBackgroundService.Configuration.Models
{
    public sealed class NotifyConfig
    {
        private List<Url> _castedLinks;

        public List<string> Links { get; set; }
        public TelegramConfig TelegramConfig { get; set; }

        public List<Url> CastedLinks 
        {
            get
            {
                if (_castedLinks == null)
                {
                    _castedLinks = new List<Url>(Links.Count);
                    Links.ForEach(linkString => _castedLinks.Add(new Url(linkString)));
                }

                return _castedLinks;
            }
        }
    }
}
