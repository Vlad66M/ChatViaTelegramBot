namespace TagHelpers_andApiTelegram.Models
{
    public class TelegramUser
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public long UserTelegramId {  get; set; }
        public string UserName { get; set; }
        public string Password {  get; set; }
    }
}
