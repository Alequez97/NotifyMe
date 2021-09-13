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

        public string Scheme
        {
            get 
            {
                return IsAbsoluteUrl ? _link.Split("://")[0] : string.Empty;
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

        public override bool Equals(object obj)
        {
            if (obj is Url url)
            {
                return url.ToString().Equals(_link);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_link, IsAbsoluteUrl, Host, Scheme);
        }

        public override string ToString()
        {
            return _link;
        }
    }
}
