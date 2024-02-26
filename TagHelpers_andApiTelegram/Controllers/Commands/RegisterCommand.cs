using System.Windows.Input;
using TagHelpers_andApiTelegram.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Microsoft.EntityFrameworkCore;

namespace TagHelpers_andApiTelegram.Controllers.Commands
{
    public class RegisterCommand : ICommand, IListener
    {


        public TelegramBotClient Client => Bot.GetTelegramBot();
        public string Name => "/Регистрация";
        public CommandExecutor Exceutor { get; }
        public RegisterCommand(CommandExecutor exceutor)
        {
            Exceutor = exceutor;
        }
        private string? password;
        private string? name;
        public async Task ExecuteAsync(Update update)
        {
            long chatId = update.Message.Chat.Id;
            Exceutor.StartListen(this);
            await Client.SendTextMessageAsync(chatId, $"Введите пароль!");
        }

        public async Task GetUpdate(Update update)
        {
            long chatId = update.Message.Chat.Id;
            if (update.Message.Text == null) return;

            if (password == null)
            {
                password = update.Message.Text;
                await Client.SendTextMessageAsync(chatId, "Введите имя!");
            }
            else
            {
                name = update.Message.Text;
                await Client.SendTextMessageAsync(chatId, "Регистрация завершена!");

                var options = new DbContextOptionsBuilder<ApplicationContext>()
                    .UseSqlite("Data Source=data.db").Options;
                using (var dbContext = new ApplicationContext(options))
                {
                    TelegramUser telegramUser = new TelegramUser();
                    telegramUser.UserTelegramId = chatId;
                    telegramUser.UserName = name;
                    telegramUser.Password = password;

                    dbContext.TelegramUsers.Add(telegramUser);

                    /*Role role0 = new Role() { Name = "admin" };
                    Role role1 = new Role() { Name = "user" };
                    Models.User user0 = new Models.User("email1@re.ru", "12345", 1, role0);
                    Models.User user1 = new Models.User("email2@re.ru", "22222", 2, role1);
                    Models.User user2 = new Models.User("email3@re.ru", "33333", 2, role1);

                    dbContext.Roles.Add(role0);
                    dbContext.Roles.Add(role1);
                    dbContext.Users.Add(user0);
                    dbContext.Users.Add(user1);
                    dbContext.Users.Add(user2);*/


                    dbContext.SaveChanges();
                }

                Exceutor.StopListen();
            }

        }
    }
}
