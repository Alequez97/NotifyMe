using LinkLookup.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LinkLookup
{
    /// <summary>
    /// This class returns only absolute links from any kind of strings.
    /// </summary>
    public class RegexLinkLookup : ILinkLookup
    {
        private Regex regex = new Regex(@"(http|https)://([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&amp;\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?", RegexOptions.IgnoreCase);

        public List<Url> GetAllLinks(string text)
        {
            return regex.Matches(text).Cast<Match>().Select(match => new Url(match.Value)).ToList();
        }
    }
}
