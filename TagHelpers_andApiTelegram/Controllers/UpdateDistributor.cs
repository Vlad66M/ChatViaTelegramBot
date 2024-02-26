using Telegram.Bot.Types;

namespace TagHelpers_andApiTelegram.Controllers
{
    public class UpdateDistributor<T> where T : ITelegramUpdateLister, new()
    {
        private Dictionary<long, T> listeners;

        public UpdateDistributor()
        {
            listeners = new Dictionary<long, T>();
        }

        public async Task GetUpdate(Update update)
        {
            long chadId = update.Message.Chat.Id;
            T? listener = listeners.GetValueOrDefault(chadId);
            if (listener is null)
            {
                listener = new T();
                listeners.Add(chadId, listener);
                await listener.GetUpdate(update);
                return;

            }
            await listener.GetUpdate(update);
        }
    }
}
