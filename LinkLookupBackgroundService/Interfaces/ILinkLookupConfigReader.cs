using MessageSender.Models;

namespace LinkLookupBackgroundService.Interfaces
{
    public interface ILinkLookupConfigReader
    {
        NotifyConfig GetGroupsConfiguration();
    }
}
