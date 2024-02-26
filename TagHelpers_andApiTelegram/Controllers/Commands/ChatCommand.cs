using Microsoft.EntityFrameworkCore;
using TagHelpers_andApiTelegram.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TagHelpers_andApiTelegram.Controllers.Commands
{
    public class ChatCommand : ICommand, IListener
    {


        public TelegramBotClient Client => Bot.GetTelegramBot();
        public string Name => "/Чат";
        public CommandExecutor Exceutor { get; }
        public ChatCommand(CommandExecutor exceutor)
        {
            Exceutor = exceutor;
        }
        private string? phone;
        private string? name;
        public async Task ExecuteAsync(Update update)
        {
            long chatId = update.Message.Chat.Id;
            try
            {
                var id = BotController.currentUsers.FirstOrDefault(tu => tu.TelegramId == chatId).Id;
            }
            catch
            {
                Exceutor.StopListen();
                await Client.SendTextMessageAsync(chatId, "Вход не выполнен!");
                return;
            }
            Exceutor.StartListen(this);
            await Client.SendTextMessageAsync(chatId, $"Введите сообщение или отправьте картинку!");
        }

        public async Task GetUpdate(Update update)
        {
            long chatId = update.Message.Chat.Id;

            TelegramBotClient bot = Bot.GetTelegramBot();
            //ChatId = update.Message.Chat.Id;
            Msg msg = new Msg();
            msg.SenderId = chatId;

            try
            {
                msg.SenderId = BotController.currentUsers.FirstOrDefault(tu => tu.TelegramId == chatId).Id;
            }
            catch
            {
                Exceutor.StopListen();
                await Client.SendTextMessageAsync(chatId, "Вход не выполнен!");
                return;
            }
            
            
            var options22 = new DbContextOptionsBuilder<ApplicationContext>()
                    .UseSqlite("Data Source=data.db").Options;
            using (var dbContext = new ApplicationContext(options22))
            {
                TelegramUser telegramUser = dbContext.TelegramUsers.FirstOrDefault(t => t.Id == msg.SenderId);

                if (telegramUser != null)
                {
                    msg.SenderName = telegramUser.UserName;
                    //msg.SenderId = telegramUser.Id;
                    
                }
                else
                {
                    //msg.SenderName = chatId.ToString();
                    Exceutor.StopListen();
                    await Client.SendTextMessageAsync(chatId, "Вход не выполнен!");
                    return;
                }
            }

            msg.Text = update.Message.Text;
            byte[]? imageBytes = null;

            try
            {
                var fileId = update.Message.Photo.Last().FileId;
                var fileInfo = await bot.GetFileAsync(fileId);
                var filePath = fileInfo.FilePath;

                const string destinationFilePath = "image.file";

                await using Stream fileStream = System.IO.File.Create(destinationFilePath);
                await bot.DownloadFileAsync(
                    filePath: filePath,
                    destination: fileStream);
                fileStream.Close();

                imageBytes = System.IO.File.ReadAllBytes(destinationFilePath);

                if (System.IO.File.Exists(destinationFilePath))
                {
                    System.IO.File.Delete(destinationFilePath);
                }
                msg.ImageBytes = imageBytes;

            }
            catch { }
            
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                    .UseSqlite("Data Source=data.db").Options;
            using (var dbContext = new ApplicationContext(options))
            {
                if (string.IsNullOrEmpty(msg.Text))
                {
                    msg.Text = "";
                }
                
                dbContext.Messages.Add(msg);
                dbContext.SaveChanges();
            }

            /*ButModel.AllMessages.Add(msg);
            ButModel.DoUpdate = true;*/
            Exceutor.StopListen();


        }
    }
}
