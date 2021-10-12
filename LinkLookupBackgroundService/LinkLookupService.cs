using CommonUtils.Interfaces;
using CommonUtils.Models;
using CommonUtils.Services;
using MessageSender.Interfaces;
using MessageSender.Models;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;
        private readonly IMessageSenderStrategy _messageSenderStrategy;
        private readonly UrlService _urlService;
        private readonly ILogger _logger;
        private List<Url> _downloadedLinks;
        private NotifyConfig _notifyConfig;

        private bool _initialLinksDownloadWasDone = false;

        /// <summary>
        /// Constructor of LinkLookupService
        /// </summary>
        public LinkLookupService(
            IConfiguration configuration,
            IConfigReader configReader,
            IMessageSenderStrategyFactory messageSenderStrategyFactory,
            UrlService urlService,
            ILogger logger
        )
        {
            _configuration = configuration;
            _urlService = urlService;
            _logger = logger;
            string configPath = $"{configuration.GetValue<string>("Workspace")}/LinkLookupBackgroundService/Configuration/aleksandrs.json";
            _notifyConfig = configReader.ReadConfigFile<NotifyConfig>(configPath);
            _messageSenderStrategy = messageSenderStrategyFactory.CreateStrategy(_notifyConfig);
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _downloadedLinks = new List<Url>();
            InitialLinksDownloadAsync();
            _messageSenderStrategy.SendMessageForAll("Service started...");

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

                            uniqueLinksToSend.ForEach(link => _messageSenderStrategy.SendMessageForAll(link.ToString()));
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
            _messageSenderStrategy.SendMessageForAll("Service stoped...");

            return base.StopAsync(cancellationToken);
        }

        private Task InitialLinksDownloadAsync()
        {
            return Task.Run(() => { 
                _notifyConfig.CastedLinks.ForEach(link => _downloadedLinks.AddRange(_urlService.DownloadLinksAsync(link).Result));
                _initialLinksDownloadWasDone = true;
            });
        }
    }
}
