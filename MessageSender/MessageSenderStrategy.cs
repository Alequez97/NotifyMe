using MessageSender.Interfaces;
using System.Collections.Generic;

namespace MessageSender
{
    public class MessageSenderStrategy : IMessageSenderStrategy
    {
        private List<IMessageSender> _messageSenders = new List<IMessageSender>();

        public void Add(IMessageSender messageSender)
        {
            _messageSenders.Add(messageSender);
        }

        public void Remove(string id)
        {
            _messageSenders.RemoveAll(ms => ms.Id == id);
        }

        public void Reset()
        {
            _messageSenders.Clear();
        }

        public void SendMessageForAll(string message)
        {
            _messageSenders.ForEach(ms => ms.SendMessage(message));
        }
    }
}
