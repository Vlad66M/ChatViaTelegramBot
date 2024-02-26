using Telegram.Bot.Types;

namespace TagHelpers_andApiTelegram.Controllers
{
    public interface IListener
    {
        public async Task GetUpdate(Update update) { }

        public CommandExecutor Exceutor { get; }
    }
}
