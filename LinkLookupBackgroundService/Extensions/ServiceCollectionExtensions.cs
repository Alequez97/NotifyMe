using LinkLookupBackgroundService.Interfaces;
using MessageSender;
using MessageSender.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace LinkLookupBackgroundService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMessageSenders(this IServiceCollection serviceCollection, ILinkLookupConfigReader configReader)
        {
            var notifyConfig = configReader.GetGroupsConfiguration();
            serviceCollection.AddSingleton<IMessageSender>(x => new TelegramMessageSender(notifyConfig.Providers.Telegram));
        }
    }
}
