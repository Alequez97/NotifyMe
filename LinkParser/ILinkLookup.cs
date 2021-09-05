using System;
using System.Collections.Generic;
using System.Text;

namespace LinkLookup
{
    public interface ILinkLookup
    {
        /// <summary>
        /// Gets all links from passed link
        /// </summary>
        /// <param name="text">Text where to lookup links</param>
        /// <returns>List of links in passed link</returns>
        public List<string> GetAllLinks(string text);
    }
}
