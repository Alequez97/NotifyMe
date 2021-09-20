using LinkLookup;
using LinkLookup.Models;
using LinkLookup.Services;
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
        private readonly UrlService _urlService;
        private List<Url> _links = new List<Url>()
        {
            new Url("https://habr.com/ru/flows/develop/")
        };
        private List<Url> _downloadedLinks;

        public LinkLookupService()
        {

        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _downloadedLinks = new List<Url>();
            _links.ForEach(link =>  _downloadedLinks.AddRange(_urlService.DownloadLinksAsync(link).Result));
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (var link in _links)
                {
                    var downloadedLinks = _urlService.DownloadLinksAsync(link).Result;
                    downloadedLinks = _urlService.RemoveAlienLinks(downloadedLinks, link);
                    downloadedLinks = _urlService.ConcatenateRelativeLinksWithHost(downloadedLinks, link);

                    var listOfLinksToSend = downloadedLinks.Where(l => _downloadedLinks.Any(dl => dl.ToString() == l.ToString()) == false).ToList();
                    // Send list of links
                    Console.WriteLine();
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
