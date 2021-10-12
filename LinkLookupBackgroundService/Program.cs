using CommonUtils.ConfigReader;
using CommonUtils.Interfaces;
using CommonUtils.Logging;
using CommonUtils.Services;
using MessageSender;
using MessageSender.Interfaces;
using Microsoft.Extensions.Configuration;
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
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<UrlService>();
                    services.AddTransient<ILinkLookup, HtmlAntlrLinkLookup>();
                    services.AddTransient<IConfigReader, JsonConfigReader>();
                    services.AddTransient<IMessageSenderStrategyFactory, MessageSenderStrategyFactory>();
                    services.AddTransient<ILogger>(x => new TextFileLogger($"{hostContext.Configuration.GetValue<string>("Workspace")}/Logs"));
                    services.AddHostedService<LinkLookupService>();
                });
        }
    }
}
