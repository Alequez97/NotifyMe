using CommonUtils.Interfaces;
using CommonUtils.Models;
using CommonUtils.Services;
using LinkLookupBackgroundService.Interfaces;
using MessageSender.Interfaces;
using MessageSender.Models;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LinkLookupBackgroundService
{
    public class LinkLookupService : BackgroundService
    {
        private readonly IEnumerable<IMessageSender> _messageSenders;
        private readonly UrlService _urlService;
        private readonly ILogger _logger;
        private List<Url> _downloadedLinks;
        private NotifyConfig _notifyConfig;

        private bool _initialLinksDownloadWasDone = false;

        /// <summary>
        /// Constructor of LinkLookupService
        /// </summary>
        public LinkLookupService(
            UrlService urlService,
            ILogger logger,
            IEnumerable<IMessageSender> messageSenders,
            ILinkLookupConfigReader notifyConfigReader
        )
        {
            _urlService = urlService;
            _logger = logger;
            _messageSenders = messageSenders;
            _notifyConfig = notifyConfigReader.GetGroupsConfiguration();
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _downloadedLinks = new List<Url>();
            InitialLinksDownloadAsync();
            try
            {
                foreach (var messageSender in _messageSenders)
                {
                    messageSender.SendMessage("Service started...");
                }
            }
            catch (Exception e)
            {
                //TODO Add exception logging
            }

            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (var link in _notifyConfig.CastedLinks)
                {
                    try
                    {
                        if (_initialLinksDownloadWasDone)
                        {
                            var downloadedLinks = await _urlService.DownloadLinksAsync(link);
                            var filteredLinks = downloadedLinks.Where(l => !_downloadedLinks.Contains(l)).ToList();
                            filteredLinks = _urlService.RemoveAlienLinks(filteredLinks, link);
                            _downloadedLinks.AddRange(filteredLinks);
                            filteredLinks = _urlService.RemoveLinksContainingSubstrings(filteredLinks, _notifyConfig.IgnoreList);
                            filteredLinks = _urlService.ConcatenateRelativeLinksWithHost(filteredLinks, link);

                            var uniqueLinksToSend = filteredLinks.Distinct().ToList();

                            uniqueLinksToSend.ForEach(link =>
                            {
                                foreach (var messageSender in _messageSenders)
                                {
                                    messageSender.SendMessage(link.ToString());
                                }
                            }
                            );
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e);
                    }
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                foreach (var messageSender in _messageSenders)
                {
                    messageSender.SendMessage("Service stoped...");
                }
            }
            catch (Exception e)
            {
                //TODO Add exception logging
            }

            return base.StopAsync(cancellationToken);
        }

        private Task InitialLinksDownloadAsync()
        {
            return Task.Run(() =>
            {
                try
                {
                    _notifyConfig.CastedLinks.ForEach(link => _downloadedLinks.AddRange(_urlService.DownloadLinksAsync(link).Result));
                    _initialLinksDownloadWasDone = true;
                }
                catch (Exception e)
                {
                    //TODO Add exception logging
                }
            });
        }
    }
}
