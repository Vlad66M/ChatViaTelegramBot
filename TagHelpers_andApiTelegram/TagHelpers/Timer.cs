using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TagHelpers_andApiTelegram.TagHelpers
{ 
    public class TimerTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Content.SetContent($"Текущее время: {@DateTime.Now.ToString("HH:mm:ss")}");


        }
    }


    public class DateTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Content.SetContent($"Текущее дата: {@DateTime.Now.ToString("DD/MM/yyyy")}");


        }
    }

    public class SumTagHelper : TagHelper
    {
        public override async Task  ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            var target = await output.GetChildContentAsync();
            var content = "<h3> Информация </h3>" + target.GetContent();
            output.Content.SetHtmlContent(content);


        }
    }
}
