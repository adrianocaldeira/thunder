using System.Web.Mvc;

namespace Thunder.Web.Mvc.Html.JavaScript
{
    /// <summary>
    /// JavaScript html builder
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class JavaScriptBuilder<TModel>
    {
        private readonly HtmlHelper<TModel> _helper;

        /// <summary>
        /// Initialize new instance of <see cref="JavaScriptBuilder{TModel}"/>
        /// </summary>
        /// <param name="helper"></param>
        public JavaScriptBuilder(HtmlHelper<TModel> helper)
        {
            _helper = helper;
        }

        /// <summary>
        /// Builder
        /// </summary>
        /// <param name="url"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
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
