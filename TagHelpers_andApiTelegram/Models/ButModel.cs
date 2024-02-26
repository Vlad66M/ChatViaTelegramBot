namespace TagHelpers_andApiTelegram.Models
{
    public class ButModel
    {
        public string? Id { get; set; }
        public int UserId {  get; set; }
        public string? ChatId { get; set; }
        public string? Text { get; set; } = "";
        public string? ImagePath { get; set; } = "";
        public IFormFile? FormFile { get; set; }

        public static List<Msg> AllMessages { get; set; } = new List<Msg>();
        public static bool DoUpdate {  get; set; }
    }
}
