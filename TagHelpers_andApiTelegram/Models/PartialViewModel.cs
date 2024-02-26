namespace TagHelpers_andApiTelegram.Models
{
    public class PartialViewModel
    {
        public List<Msg> AllMessages { get; set; } = new List<Msg>();
        public bool DoUpdate {  get; set; }
    }
}
