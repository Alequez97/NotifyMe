using CommonUtils.Interfaces;
using CommonUtils.Logging;
using CommonUtils.Services;
using LinkLookupBackgroundService.ConfigurationReaders;
using LinkLookupBackgroundService.Extensions;
using LinkLookupBackgroundService.Interfaces;
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
                    services.AddTransient<ILinkLookupConfigReader>(x => new JsonLinkLookupConfigReader(hostContext.Configuration, args[0]));

                    if (args.Length < 1)
                    {
                        throw new InvalidOperationException("Group name command line argument is mandatory");
                    }

                    services.AddMessageSenders(new JsonLinkLookupConfigReader(hostContext.Configuration, args[0]));

                    services.AddTransient<ILogger>(x => 
                        new TextFileLogger($"{hostContext.Configuration.GetValue<string>("Workspace")}/Logs"));

                    services.AddHostedService<LinkLookupService>();
                });
        }
    }
}
