using CommonUtils.ConfigReader;
using LinkLookupBackgroundService.Interfaces;
using MessageSender.Models;
using Microsoft.Extensions.Configuration;
using System;

namespace LinkLookupBackgroundService.ConfigurationReaders
{
    public class JsonLinkLookupConfigReader : ILinkLookupConfigReader
    {
        private readonly string _configPath;

        public JsonLinkLookupConfigReader(string groupName)
        {
            _configPath = $"{Environment.CurrentDirectory}/Configuration/{groupName}.json";
        }

        public NotifyConfig GetGroupsConfiguration()
        {
            return new JsonConfigReader().ReadConfigFile<NotifyConfig>(_configPath);
        }
    }
}
