using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkLookup.Models
{
    public class Url
    {
        private readonly string _link;

        public Url(string link)
        {
            _link = link;
        }

        public bool IsAbsoluteUrl
        {
            get
            {
                return _link.Contains("http");
            }
        }

        public string Host
        {
            get
            {
                return IsAbsoluteUrl ? GetHost() : string.Empty;
            }
        }

        private string GetHost()
        {
            var linkWithoutSchema = _link.Split("://")[1];
            var linkParts = linkWithoutSchema.Split(".");
            var domainName = linkParts[0];
            var topLevelDomain = linkParts[1].Split("/")[0];
            var result = $"{domainName}.{topLevelDomain}";
            return result;
        }
    }
}
