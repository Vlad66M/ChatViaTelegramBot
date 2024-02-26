using Microsoft.EntityFrameworkCore;
using TagHelpers_andApiTelegram.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TagHelpers_andApiTelegram.Controllers.Commands
{
    public class LoginCommand : ICommand, IListener
    {


        public TelegramBotClient Client => Bot.GetTelegramBot();
        public string Name => "/Войти";
        public CommandExecutor Exceutor { get; }
        public LoginCommand(CommandExecutor exceutor)
        {
            Exceutor = exceutor;
        }
        private string? password;
        private string? name;
        public async Task ExecuteAsync(Update update)
        {
            long chatId = update.Message.Chat.Id;
            Exceutor.StartListen(this);
            await Client.SendTextMessageAsync(chatId, $"Введите имя!");
        }

        public async Task GetUpdate(Update update)
        {
            long chatId = update.Message.Chat.Id;
            if (update.Message.Text == null) return;

            if (name == null)
            {
                name = update.Message.Text;
                await Client.SendTextMessageAsync(chatId, "Введите пароль!");
            }
            else
            {
                password = update.Message.Text;

                var options = new DbContextOptionsBuilder<ApplicationContext>()
                    .UseSqlite("Data Source=data.db").Options;
                using (var dbContext = new ApplicationContext(options))
                {
                    TelegramUser telegramUser = dbContext.TelegramUsers.FirstOrDefault(u => u.UserName == name && u.Password == password);
                    if (telegramUser == null)
                    {
                        await Client.SendTextMessageAsync(chatId, "Неверные логин или пароль!");
                        Exceutor.StopListen();
                    }
                    else
                    {
                        await Client.SendTextMessageAsync(chatId, "Вход выполнен!");
                        TU newTU = new TU();
                        newTU.TelegramId = chatId;
                        newTU.Id = telegramUser.Id;
                        newTU.TelegramId = chatId;
                        newTU.Id = telegramUser.Id;
                        TU current = BotController.currentUsers.FirstOrDefault(tu => tu.TelegramId == chatId);
                        if(current == null)
                        {
                            BotController.currentUsers.Add(newTU);
                        }
                        else
                        {
                            current.TelegramId = chatId;
                            current.Id = telegramUser.Id;
                        }
                    }
                }
                name = null;
                password = null;

                Exceutor.StopListen();
            }

        }
    }
}
