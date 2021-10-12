using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageSender.Interfaces
{
    public interface IMessageSenderStrategy
    {
        void Add(IMessageSender messageSender);

        void Remove(string id);

        void Reset();

        void SendMessageForAll(string message);
    }
}
