using MessageSender.Models;

namespace MessageSender.Interfaces
{
    public interface IMessageSenderStrategyFactory
    {
        public IMessageSenderStrategy CreateStrategy(NotifyConfig config);
    }
}
