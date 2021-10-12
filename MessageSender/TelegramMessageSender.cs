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
        private readonly Models.TelegramConfig _telegramConfig;
        private TelegramBotClient _telegramBot;

        public TelegramMessageSender(Models.TelegramConfig telegramConfig)
        {
            _telegramConfig = telegramConfig;
            InitializeTelegraBotClient();
        }

        public TelegramMessageSender(string configFilePath, IConfigReader configReader)
        {
            _telegramConfig = configReader.ReadConfigFile<Models.TelegramConfig>(configFilePath);
            InitializeTelegraBotClient();
        }

        public TelegramMessageSender(string botsToken, string userId)
        {
            _telegramConfig = new Models.TelegramConfig()
            {
                Token = botsToken,
                ChatId = userId
            };
            InitializeTelegraBotClient();
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

        private void InitializeTelegraBotClient()
        {
            if (_telegramBot == null)
            {
                _telegramBot = new TelegramBotClient(_telegramConfig.Token);
            }
        }
    }
}
