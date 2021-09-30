using CommonUtils.ConfigReader;
using CommonUtils.Logging;
using LinkLookup.Models;
using LinkLookup.Services;
using LinkLookupBackgroundService.Configuration.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

namespace LinkLookupBackgroundService
{
    public class LinkLookupService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly UrlService _urlService;
        private readonly IConfigReader _configReader;
        private readonly ILogger _logger;
        private List<Url> _downloadedLinks;

        private TelegramBotClient _telegramBot;
        private NotifyConfig _notifyConfig;

        private bool _configIsSuccessfullyInitialized;

        /// <summary>
        /// Constructor of LinkLookupService
        /// </summary>
        public LinkLookupService(
            IConfiguration configuration,
            UrlService urlService,
            IConfigReader configReader,
            ILogger logger
        )
        {
            _configuration = configuration;
            _urlService = urlService;
            _configReader = configReader;
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            LoadConfigAsync().ContinueWith((task) => 
            {
                _downloadedLinks = new List<Url>();
                try
                {
                    if (_configIsSuccessfullyInitialized)
                    {
                        _notifyConfig.CastedLinks.ForEach(link => _downloadedLinks.AddRange(_urlService.DownloadLinksAsync(link).Result));
                    }
                    else
                    {
                        _logger.LogInfo("StartAsync: Config is not initialized");
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e);
                }
            });
            
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_configIsSuccessfullyInitialized)
                {
                    foreach (var link in _notifyConfig.CastedLinks)
                    {
                        try
                        {
                            var downloadedLinks = _urlService.DownloadLinksAsync(link).Result;
                            var filteredLinks = downloadedLinks.Where(l => !_downloadedLinks.Contains(l)).ToList();
                            filteredLinks = _urlService.RemoveAlienLinks(filteredLinks, link);
                            _downloadedLinks.AddRange(filteredLinks);
                            filteredLinks = _urlService.RemoveLinksContainingSubstrings(filteredLinks, _notifyConfig.IgnoreList);
                            filteredLinks = _urlService.ConcatenateRelativeLinksWithHost(filteredLinks, link);

                            var uniqueLinksToSend = filteredLinks.Distinct().ToList();

                            uniqueLinksToSend.ForEach(async link => await _telegramBot.SendTextMessageAsync(_notifyConfig.TelegramConfig.ChatId, link.ToString()));
                        }
                        catch (Exception e)
                        {
                            _logger.LogError(e);
                        }
                    }
                }
                else
                {
                    _logger.LogInfo("ExecuteAsync: Config is not initialized");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        private Task LoadConfigAsync()
        {
            return Task.Run(() => {
                try
                {
                    var workspacePath = _configuration.GetValue<string>("Workspace");
                    var configPath = $"{workspacePath}/LinkLookupBackgroundService/Configuration/romanich.json";
                    _notifyConfig = _configReader.ReadConfigFile<NotifyConfig>(configPath);

                    _telegramBot = new TelegramBotClient(_notifyConfig.TelegramConfig.Token);
                    _telegramBot.SendTextMessageAsync(_notifyConfig.TelegramConfig.ChatId, "Service started...").Wait();
                    _telegramBot.SendTextMessageAsync(_notifyConfig.TelegramConfig.ChatId, "Link you subscribed on:").Wait();
                    _notifyConfig.CastedLinks.ForEach(link => {
                        _telegramBot.SendTextMessageAsync(_notifyConfig.TelegramConfig.ChatId, link.ToString());
                    });
                    
                    _configIsSuccessfullyInitialized = true;
                }
                catch (Exception e)
                {
                    _logger.LogError(e);
                    _configIsSuccessfullyInitialized = false;
                }
            });
        }
    }
}
