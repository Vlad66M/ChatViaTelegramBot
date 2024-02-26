namespace TagHelpers_andApiTelegram
{
    public class Msg
    {
        public int Id {  get; set; }
        public long SenderId {  get; set; }
        public string SenderName {  get; set; }
        public string Text { get; set; }
        public byte[]? ImageBytes { get; set; } = null;
    }
}
