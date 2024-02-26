using Telegram.Bot;
using Telegram.Bot.Types;

namespace TagHelpers_andApiTelegram.Controllers.Commands
{
    public interface ICommand
    {
        public TelegramBotClient Client { get; }

        public string Name { get; }

        public async Task ExecuteAsync(Update update) { }

    }
}
