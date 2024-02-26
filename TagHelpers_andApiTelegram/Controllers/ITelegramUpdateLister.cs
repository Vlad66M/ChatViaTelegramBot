using Telegram.Bot.Types;

namespace TagHelpers_andApiTelegram.Controllers
{
    public interface ITelegramUpdateLister
    {
        Task GetUpdate(Update update);
    }
}
