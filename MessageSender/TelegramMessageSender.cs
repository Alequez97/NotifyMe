using CommonUtils.Interfaces;
using MessageSender.Interfaces;
using MessageSender.Models;
using System;
using System.Threading.Tasks;
using Telegram.Bot;

namespace MessageSender
{
    public class TelegramMessageSender : IMessageSender
    {
        private readonly TelegramConfig _telegramConfig;
        private TelegramBotClient _telegramBot;

        public TelegramMessageSender(TelegramConfig telegramConfig)
        {
            _telegramConfig = telegramConfig;
            if (_telegramBot == null)
            {
                _telegramBot = new TelegramBotClient(_telegramConfig.Token, httpClient: null, null);
            }
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();

        public void SendMessage(string message)
        {
            _telegramBot.SendTextMessageAsync(_telegramConfig.ChatId, message).Wait();
        }

        public Task SendMessageAsync(string message)
        {
            return _telegramBot.SendTextMessageAsync(_telegramConfig.ChatId, message);
        }
    }
}
