using LinkLookup;
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
        private IEnumerable<Uri> _links = new List<Uri>()
        {
            new Uri("https://habr.com/ru/flows/develop/")
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
                    var htmlResponse = await _httpClient.GetStringAsync(link);
                    var downloadedLinks = _linkLookup.GetAllLinks(htmlResponse);
                    downloadedLinks = RemoveLinksFromForeignHost(downloadedLinks, link);

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

        private List<string> RemoveLinksFromForeignHost(List<string> links, Uri usersUri)
        {
            var newLinks = new List<string>();
            for (int i = 0; i < links.Count; i++)
            {
                var link = links[i];
                if (IsAbsoluteUri(link) && GetLinksHost(link) == usersUri.Host)
                {
                    newLinks.Add(link);
                }
                
                if (!IsAbsoluteUri(link))
                {
                    link = (link[0] == '/') ? link.Substring(1) : link;
                    newLinks.Add($"{usersUri.Scheme}://{usersUri.Host}/{link}");
                }
            }

            return newLinks;
        }

        private bool IsAbsoluteUri(string link)
        {
            var result = link.Contains("http");
            return result;
        }

        private string GetLinksHost(string link)
        {
            if (!IsAbsoluteUri(link))
            {
                return string.Empty;
            }
            else
            {
                var linkWithoutSchema = link.Split("://")[1];
                var linkParts = linkWithoutSchema.Split(".");
                var domainName = linkParts[0];
                var topLevelDomain = linkParts[1].Split("/")[0];
                var result = $"{domainName}.{topLevelDomain}";
                return result;
            }
        }
    }
}
