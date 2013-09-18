using System.Web.Mvc;

namespace Thunder.Web.Mvc.Html.Image
{
    public class ImageBuilder<TModel>
    {
        private readonly HtmlHelper<TModel> _helper;

        public ImageBuilder(HtmlHelper<TModel> helper)
        {
            _helper = helper;
        }

        public MvcHtmlString Builder(string url, object htmlAttributes)
        {
            var tagBuilder = new TagBuilder("img");
            var attributes = HtmlAttributesUtility.ObjectToHtmlAttributesDictionary(htmlAttributes);

            attributes.Add("border", "0");
            attributes.Add("src", UrlHelper.GenerateContentUrl(url, _helper.ViewContext.HttpContext));

            tagBuilder.MergeAttributes(attributes);

            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }
    }
}
