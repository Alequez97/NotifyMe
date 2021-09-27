using CommonUtils.ConfigReader;
using CommonUtils.Logging;
using LinkLookup;
using LinkLookup.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

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
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Local.json", optional: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddCommandLine(args)
                .Build();

            return Host.CreateDefaultBuilder(args)
                    .UseWindowsService()
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddTransient<UrlService>();
                        services.AddTransient<IConfigReader, JsonConfigReader>();
                        services.AddTransient<ILogger>(x => new TextFileLogger($"{hostContext.Configuration.GetValue<string>("Workspace")}/Logs"));
                        services.AddTransient<ILinkLookup, HtmlAntlrLinkLookup>();
                        services.AddHostedService<LinkLookupService>();
                    });
        }
            
    }
}
