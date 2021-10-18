using CommonUtils.Interfaces;
using CommonUtils.Logging;
using CommonUtils.Services;
using LinkLookupBackgroundService.ConfigurationReaders;
using LinkLookupBackgroundService.Interfaces;
using MessageSender;
using MessageSender.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace LinkLookupBackgroundService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<UrlService>();
                    services.AddTransient<ILinkLookup, HtmlAntlrLinkLookup>();
                    services.AddTransient<IMessageSenderStrategyFactory, MessageSenderStrategyFactory>();
                    services.AddTransient<ILogger>(x => 
                        new TextFileLogger($"{hostContext.Configuration.GetValue<string>("Workspace")}/Logs"));

                    if (args.Length < 1)
                    {
                        throw new InvalidOperationException("Group name command line argument is mandatory");
                    }
                    services.AddTransient<ILinkLookupConfigReader>(x =>
                        new MongoDbLinkLookupConfigReader(args[0]));

                    services.AddHostedService<LinkLookupService>();
                });
        }
    }
}
