using LinkLookup;
using LinkLookup.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LinkLookupBackgroundService
{
    public class LinkLookupService : BackgroundService
    {
        private readonly ILinkLookup _linkLookup;
        private IEnumerable<Url> _links = new List<Url>()
        {
            new Url("https://habr.com/ru/flows/develop/")
        };
        private HttpClient _httpClient;

        public LinkLookupService(ILinkLookup linkLookup)
        {
            _linkLookup = linkLookup;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _httpClient = new HttpClient();
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (var link in _links)
                {
                    var htmlResponse = await _httpClient.GetStringAsync(link.ToString());
                    var downloadedLinks = _linkLookup.GetAllLinks(htmlResponse);
                    downloadedLinks = ModifyLinksList(downloadedLinks, link);

                    File.AppendAllLines(@"C:\Tmp\links.txt", downloadedLinks.Select(x => x.ToString()));
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _httpClient.Dispose();
            return base.StopAsync(cancellationToken);
        }

        /// <summary>
        /// Method that removes links from another host
        /// and concatenates
        /// </summary>
        /// <param name="links"></param>
        /// <param name="usersUrl"></param>
        /// <returns></returns>
        private List<string> ModifyLinksList(List<string> links, Url usersUrl)
        {
            var newLinks = new List<string>();
            for (int i = 0; i < links.Count; i++)
            {
                var link = links[i];
                var downloadedUrl = new Url(link);
                if (downloadedUrl.IsAbsoluteUrl && downloadedUrl.Equals(usersUrl))
                {
                    newLinks.Add(link);
                }
                
                if (!downloadedUrl.IsAbsoluteUrl)
                {
                    link = (link[0] == '/') ? link[1..] : link;
                    newLinks.Add($"{usersUrl.Scheme}://{usersUrl.Host}/{link}");
                }
            }

            return newLinks;
        }
    }
}
