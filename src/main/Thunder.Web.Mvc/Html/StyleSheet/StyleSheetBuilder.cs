using System.Web.Mvc;

namespace Thunder.Web.Mvc.Html.StyleSheet
{
    /// <summary>
    /// StyleSheet html builder
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class StyleSheetBuilder<TModel>
    {
        private readonly HtmlHelper<TModel> _helper;

        /// <summary>
        /// Initialize new instance of <see cref="StyleSheetBuilder{TModel}"/>
        /// </summary>
        /// <param name="helper"></param>
        public StyleSheetBuilder(HtmlHelper<TModel> helper)
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
            var tagBuilder = new TagBuilder("link");
            var attributes = HtmlAttributesUtility.ObjectToHtmlAttributesDictionary(htmlAttributes);

            tagBuilder.Attributes.Add("type", "text/css");
            tagBuilder.Attributes.Add("media", "screen");
            tagBuilder.Attributes.Add("rel", "stylesheet");
            tagBuilder.Attributes.Add("href", UrlHelper.GenerateContentUrl(url, _helper.ViewContext.HttpContext));

            tagBuilder.MergeAttributes(attributes, true);
            
            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }
    }
}
