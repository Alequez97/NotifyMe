using CommonUtils.Interfaces;
using MessageSender.Interfaces;
using MessageSender.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageSender
{
    public class MessageSenderStrategyFactory : IMessageSenderStrategyFactory
    {
        public IMessageSenderStrategy CreateStrategy(NotifyConfig config)
        {
            var strategy = new MessageSenderStrategy();
            if (config.Providers.Telegram != null)
            {
                strategy.Add(new TelegramMessageSender(config.Providers.Telegram));
            }

            return strategy;
        }
    }
}
