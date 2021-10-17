using CommonUtils.ConfigReader;
using LinkLookupBackgroundService.Interfaces;
using MessageSender.Models;
using Microsoft.Extensions.Configuration;

namespace LinkLookupBackgroundService.ConfigurationReaders
{
    public class JsonLinkLookupConfigReader : ILinkLookupConfigReader
    {
        private readonly IConfiguration _configuration;
        private readonly string _groupName;

        public JsonLinkLookupConfigReader(IConfiguration configuration, string groupName)
        {
            _configuration = configuration;
            _groupName = groupName;
        }

        public NotifyConfig GetGroupsConfiguration()
        {
            var configPath = $"{_configuration.GetValue<string>("Workspace")}/LinkLookupBackgroundService/Configuration/{_groupName}.json";
            var notifyConfig = new JsonConfigReader().ReadConfigFile<NotifyConfig>(configPath);

            return notifyConfig;
        }
    }
}
