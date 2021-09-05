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
        private IEnumerable<string> _links = new List<string>()
        {
            "https://habr.com/ru/flows/develop/"
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
                    var allLinksInPage = _linkLookup.GetAllLinks(htmlResponse);
                    allLinksInPage.Insert(0, "*****************");
                    allLinksInPage.Insert(0, $"\n{DateTime.Now}");

                    File.AppendAllLines(@"C:\Tmp\links.txt", allLinksInPage);
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _httpClient.Dispose();
            return base.StopAsync(cancellationToken);
        }
    }
}
