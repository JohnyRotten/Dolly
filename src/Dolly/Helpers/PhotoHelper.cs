using Microsoft.AspNet.Razor.TagHelpers;

namespace Dolly.Helpers
{
    [HtmlTargetElement(tag: "img", Attributes = "photo")]
    public class PhotoHelper : TagHelper
    {

        public string Photo { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.RemoveAll("photo");
            output.Attributes.Add("data-aload", $"/Photos/Show/{Photo}");
        }
    }
}