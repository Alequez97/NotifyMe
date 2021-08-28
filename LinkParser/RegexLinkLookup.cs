using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LinkLookup
{
    public class RegexLinkLookup : ILinkLookup
    {
        private Regex regex = new Regex(@"http://([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&amp;\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?", RegexOptions.IgnoreCase);

        public List<string> GetAllLinks(string link)
        {
            return regex.Matches(link).Cast<Match>().Select(match => match.Value).ToList();
        }
    }
}
