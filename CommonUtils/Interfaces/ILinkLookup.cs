using CommonUtils.Models;
using System.Collections.Generic;

namespace CommonUtils.Interfaces
{
    public interface ILinkLookup
    {
        /// <summary>
        /// Gets all links from passed link
        /// </summary>
        /// <param name="text">Text where to lookup links</param>
        /// <returns>List of links in passed link</returns>
        public List<Url> GetAllLinks(string text);
    }
}
