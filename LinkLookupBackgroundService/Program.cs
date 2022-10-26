using CommonUtils.Interfaces;
using CommonUtils.Logging;
using CommonUtils.Services;
using LinkLookupBackgroundService.ConfigurationReaders;
using LinkLookupBackgroundService.Extensions;
using LinkLookupBackgroundService.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
                .UseSystemd()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<UrlService>();
                    services.AddTransient<ILinkLookup, HtmlAntlrLinkLookup>();

                    var linkLookupImplementation = new JsonLinkLookupConfigReader("aleksandrs");

                    services.AddTransient<ILinkLookupConfigReader>(x => linkLookupImplementation);

                    services.AddMessageSenders(linkLookupImplementation);

                    services.AddTransient<ILogger, DumpLogger>();

                    services.AddHostedService<LinkLookupService>();
                });
        }
    }
}
