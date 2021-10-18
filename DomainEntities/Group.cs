using MessageSender.Models;

namespace DomainEntites
{
    public class Group : ModelBase
    {
        public string Name { get; set; }

        public NotifyConfig NotifyConfig { get; set; }
    }
}
