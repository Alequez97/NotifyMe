using LinkLookup;
using LinkLookup.Models;
using LinkLookup.Services;
using LinkLookupBackgroundService.Configuration.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
        private List<Url> _links = new List<Url>()
        {
            new Url("https://habr.com/ru/flows/develop/"),
            new Url("https://www.ixbt.com/")
        };
        private List<Url> _downloadedLinks;

        private TelegramBotClient _telegramBot;
        private NotifyConfig _notifyConfig;

        private bool _configIsSuccessfullyInitialized;

        public LinkLookupService(IConfiguration configuration, UrlService urlService)
        {
            _configuration = configuration;
            _urlService = urlService;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            LoadConfigAsync();
            _downloadedLinks = new List<Url>();
            _links.ForEach(link => _downloadedLinks.AddRange(_urlService.DownloadLinksAsync(link).Result));
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (var link in _links)
                {
                    //try
                    //{
                    var downloadedLinks = await _urlService.DownloadLinksAsync(link);
                    _downloadedLinks.AddRange(downloadedLinks);
                    var filteredLinks = downloadedLinks.Where(l => !_downloadedLinks.Contains(l)).ToList();
                    filteredLinks = _urlService.RemoveAlienLinks(filteredLinks, link);
                    filteredLinks = _urlService.ConcatenateRelativeLinksWithHost(filteredLinks, link);

                    var uniqueLinksToSend = filteredLinks.Distinct().ToList();

                    if (_configIsSuccessfullyInitialized)
                    {
                        uniqueLinksToSend.ForEach(async link => await _telegramBot.SendTextMessageAsync(_notifyConfig.TelegramConfig.ChatId, link.ToString()));
                    }
                    else
                    {
                        File.AppendAllText("C:/Tmp/log.txt", $"[{DateTime.Now}] Config is not initialized\n");
                    }
                    //}
                    //catch (Exception e)
                    //{
                    //    // TODO: Add logging to log exception 
                    //}
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
            return Task.Run(() =>
            {
                try
                {
                    var workspacePath = _configuration.GetValue<string>("Workspace");
                    File.AppendAllText("C:/Tmp/workspace.txt", $"[{DateTime.Now}] Workspace: {workspacePath}\n");
                    var notifyConfigJson = File.ReadAllText($"{workspacePath}/LinkLookupBackgroundService/Configuration/aleksandrs.json");
                    _notifyConfig = JsonConvert.DeserializeObject<NotifyConfig>(notifyConfigJson);

                    _telegramBot = new TelegramBotClient(_notifyConfig.TelegramConfig.Token);
                    _telegramBot.SendTextMessageAsync(_notifyConfig.TelegramConfig.ChatId, "Service started...");
                    _configIsSuccessfullyInitialized = true;
                }
                catch (Exception e)
                {
                    // TODO: Add logging to log exception 
                    _configIsSuccessfullyInitialized = false;
                }
            });
        }
    }
}
