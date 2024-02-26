using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.IO.Pipes;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml.Linq;
using TagHelpers_andApiTelegram.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TagHelpers_andApiTelegram.Controllers
{
    public class TU
    {
        public int Id { get; set; }
        public long TelegramId {  get; set; }
    }

    [ApiController]
    [Route("/")]
    public class BotController : Controller
    {
        public static List<TU> currentUsers = new List<TU>();

        private static UpdateDistributor<CommandExecutor> _distributor = new UpdateDistributor<CommandExecutor>();

        private readonly ILogger<HomeController> _logger;
        static long ChatId;
        private int messagesNumber = ButModel.AllMessages.Count;
        /*static List<Msg> Messages = new List<Msg>();*/
        public BotController(ILogger<HomeController> logger)
        {

            _logger = logger;
        }
        [HttpPost]
        public async void Post(Update update)
        {
            if (update.Message == null) return;

            await _distributor.GetUpdate(update);

            /*TelegramBotClient bot = Bot.GetTelegramBot();
            ChatId = update.Message.Chat.Id;
            Msg msg = new Msg();
            msg.SenderName = "Telegram";
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

            MessageModel.AllMessages.Add(msg);
            MessageModel.DoUpdate = true;*/




        }

        [HttpGet]
        public IActionResult Get()
        {
            return View(); 
        }

        [Authorize(Roles = "admin, user")]
        [HttpGet("Bot/But/{id?}")]
        public IActionResult But(string? id)
        {
            ButModel bm = new ButModel();
            if (id != null)
            {
                bm.Id = id;
                ViewBag.Id = id;
                TempData["Id"] = id;
            }
            else
            {
                ViewBag.Id = "null";
                TempData["Id"] = "null";
            }
            /*return View(bm);*/
            return RedirectToAction("But");
        }
        

        [Authorize(Roles = "admin, user")]
        [HttpPost("Bot/But/{id?}")]
        /*[Consumes("multipart/form-data")]*/
        /*[RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]*/
        public   async Task<IActionResult> But([FromForm] ButModel mes)
        {
            if(mes.Id != null)
            {
                Msg msg = new Msg();
                msg.SenderId = long.Parse(mes.Id);
                msg.SenderName = "Вы";
                long cid = 0;
                var options2 = new DbContextOptionsBuilder<ApplicationContext>()
                    .UseSqlite("Data Source=data.db").Options;
                TelegramBotClient bot = Bot.GetTelegramBot();
                using (var dbContext = new ApplicationContext(options2))
                {
                    TelegramUser telegramUser = dbContext.TelegramUsers.FirstOrDefault(u => u.Id == int.Parse(mes.Id));
                    if (telegramUser != null)
                    {
                        cid = telegramUser.UserTelegramId;
                    }
                }
                if (!string.IsNullOrEmpty(mes.Text))
                {
                    await bot.SendTextMessageAsync(cid, mes.Text);
                }
                
                if (!string.IsNullOrEmpty(mes.Text))
                {
                    msg.Text = mes.Text;

                    //ButModel.AllMessages.Add(msg);
                    
                    

                }

                if (mes.FormFile != null)
                {
                    try
                    {
                        var stream = mes.FormFile.OpenReadStream();

                        MemoryStream ms = new MemoryStream();
                        stream.CopyTo(ms);
                        byte[] byteArray = ms.ToArray();
                        msg.ImageBytes = byteArray;
                        ms.Close();

                        Stream stream2 = new MemoryStream(byteArray);

                        if (string.IsNullOrEmpty(msg.Text))
                        {
                            msg.Text = "";
                        }

                        //byte[] imageBytes = System.IO.File.ReadAllBytes(mes.ImagePath);
                        //Stream stream = new MemoryStream(imageBytes);
                        TelegramBotClient bot2 = Bot.GetTelegramBot();
                        await bot2.SendPhotoAsync(
                        chatId: cid,
                        photo: InputFile.FromStream(stream2),
                        parseMode: ParseMode.Html);
                        /*try
                        {
                            stream.Close();
                        }
                        catch { }*/


                        



                        //ButModel.AllMessages.Add(msg);
                    }
                    catch { }
                    
                }

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
            }
            
            
            return View();
        }

        public static MemoryStream CopyToMemory(Stream input)
        {
            // It won't matter if we throw an exception during this method;
            // we don't *really* need to dispose of the MemoryStream, and the
            // caller should dispose of the input stream
            MemoryStream ret = new MemoryStream();

            byte[] buffer = new byte[99999999];
            int bytesRead;
            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                ret.Write(buffer, 0, bytesRead);
            }
            // Rewind ready for reading (typical scenario)
            ret.Position = 0;
            return ret;
        }
        public byte[] UseStreamReader(Stream stream)
        {
            byte[] bytes;
            using (var reader = new StreamReader(stream))
            {
                bytes = System.Text.Encoding.UTF8.GetBytes(reader.ReadToEnd());
            }
            return bytes;
        }

        [HttpGet("Bot/PartialBut/{id?}")]
        public async Task<IActionResult> PartialBut(string? id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                PartialViewModel partialViewModel = new PartialViewModel();
                partialViewModel.DoUpdate = false;

                var options = new DbContextOptionsBuilder<ApplicationContext>()
                    .UseSqlite("Data Source=data.db").Options;
                using (var dbContext = new ApplicationContext(options))
                {
                    partialViewModel.AllMessages = await dbContext.Messages.Where(m => m.SenderId == int.Parse(id)).ToListAsync();
                }

                return PartialView("MessagesList", partialViewModel.AllMessages);
            }
            else
            {
                return NotFound();
            }
            
        }

    }
}
