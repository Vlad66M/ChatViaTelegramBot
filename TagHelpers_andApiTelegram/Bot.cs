using Telegram.Bot;

public class Bot
{
    private static TelegramBotClient client { get; set; }

    public static TelegramBotClient GetTelegramBot()
    {
        if (client != null)
        {
            return client;
        }
        client = new TelegramBotClient("6509157512:AAHxJVeYx65ffSk6MYqToBQEBdQZBjOXhWY");
        return client;
    }
}
