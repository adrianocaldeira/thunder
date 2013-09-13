using System.Web.Mvc;

namespace Thunder.Web.Mvc.Html.JavaScript
{
    public class JavaScriptBuilder<TModel>
    {
        private readonly HtmlHelper<TModel> _helper;

        public JavaScriptBuilder(HtmlHelper<TModel> helper)
        {
            _helper = helper;
        }

        public MvcHtmlString Builder(string url, object htmlAttributes)
        {
            var tagBuilder = new TagBuilder("script");
            var attributes = HtmlAttributesUtility.ObjectToHtmlAttributesDictionary(htmlAttributes);

            tagBuilder.Attributes.Add("type", "text/javascript");
            tagBuilder.Attributes.Add("src", UrlHelper.GenerateContentUrl(url, _helper.ViewContext.HttpContext));

            tagBuilder.MergeAttributes(attributes, true);

            return MvcHtmlString.Create(tagBuilder.ToString());
        }
    }
}
