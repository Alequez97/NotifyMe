using System;
using System.Threading.Tasks;

namespace MessageSender.Interfaces
{
    public interface IMessageSender
    {
        public string Id { get; }
        void SendMessage(string message);
        Task SendMessageAsync(string message);
    }
}
