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

        public string Subdomain
        {
            get
            {
                return IsAbsoluteUrl ? GetSubdomain() : string.Empty;
            }
        }

        private string GetHost()
        {
            var linkWithoutSchema = _link.Split("://")[1];
            if (!string.IsNullOrEmpty(Subdomain))
            {
                linkWithoutSchema = linkWithoutSchema.Replace($"{Subdomain}.", "");
            }

            linkWithoutSchema = linkWithoutSchema.Split("?")[0];
            linkWithoutSchema = linkWithoutSchema.Split("/")[0];
            return linkWithoutSchema;
        }

        private string GetSubdomain()
        {
            var linkWithoutSchema = _link.Split("://")[1];
            if (linkWithoutSchema.Count(@char => @char == '.') >= 2)
            {
                return linkWithoutSchema.Split(".")[0];
            }

            return string.Empty;
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
