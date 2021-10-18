using CommonUtils.Models;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MessageSender.Models
{
    public sealed class NotifyConfig
    {
        private List<Url> _castedLinks;

        public List<string> Links { get; set; }
        public List<string> IgnoreList { get; set; }
        public ProvidersConfig Providers { get; set; }

        [JsonIgnore]
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
