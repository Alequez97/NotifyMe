using LinkLookup.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LinkLookup.Services
{
    public class UrlService
    {
        private readonly ILinkLookup _linkLookup;

        public UrlService(ILinkLookup linkLookup)
        {
            _linkLookup = linkLookup;
        }

        public async Task<List<Url>> DownloadLinksAsync(Url url)
        {
            using var httpClient = new HttpClient();
            var htmlResponse = await httpClient.GetStringAsync(url.ToString());
            var downloadedLinks = _linkLookup.GetAllLinks(htmlResponse);

            return downloadedLinks;
        }

        /// <summary>
        /// Method concatenates relative links with host of url passed as parameter
        /// </summary>
        /// <param name="links"></param>
        /// <param name="url"></param>
        /// <returns>Modified list of urls</returns>
        public List<Url> ConcatenateRelativeLinksWithHost(List<Url> links, Url url)
        {
            var newLinks = new List<Url>();
            for (int i = 0; i < links.Count; i++)
            {
                var downloadedUrl = links[i];

                if (!downloadedUrl.IsAbsoluteUrl)
                {
                    var link = downloadedUrl.ToString();
                    link = (link[0] == '/') ? link[1..] : link;
                    if (url.ToString().Contains("www"))
                    {
                        newLinks.Add(new Url($"{url.Scheme}://www.{url.Host}/{link}"));
                    }
                    else
                    {
                        newLinks.Add(new Url($"{url.Scheme}://{url.Host}/{link}"));
                    }
                }
                if (downloadedUrl.IsAbsoluteUrl)
                {
                    newLinks.Add(downloadedUrl);
                }
            }

            return newLinks;
        }

        /// <summary>
        /// Method that removes links from alien host
        /// </summary>
        /// <param name="links">List of LinkLookup.Model.Url</param>
        /// <param name="url">Users specified url</param>
        /// <returns>Modified list of urls</returns>
        public List<Url> RemoveAlienLinks(List<Url> links, Url url)
        {
            var newLinks = new List<Url>();
            for (int i = 0; i < links.Count; i++)
            {
                var downloadedUrl = links[i];
                if (downloadedUrl.IsAbsoluteUrl && downloadedUrl.Host.Equals(url.Host))
                {
                    newLinks.Add(downloadedUrl);
                }
                if (!downloadedUrl.IsAbsoluteUrl)
                {
                    newLinks.Add(downloadedUrl);
                }
            }

            return newLinks;
        }

        public List<Url> RemoveLinksContainingSubstrings(List<Url> links, List<string> substrings)
        {
            for (int i = 0; i < substrings.Count; i++)
            {
                var substring = substrings[i];
                for (int j = 0; j < links.Count; j++)
                {
                    var link = links[j];
                    if (link.ToString().Contains(substring))
                    {
                        links.Remove(link);
                        j--;
                    }
                }
            }

            return links;
        }
    }
}
