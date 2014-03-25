using System.Web.Mvc;

namespace Thunder.Web.Mvc.Html.Image
{
    /// <summary>
    /// Image html builder
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class ImageBuilder<TModel>
    {
        private readonly HtmlHelper<TModel> _helper;

        /// <summary>
        /// Initialize new instance of <see cref="ImageBuilder{TModel}"/>
        /// </summary>
        /// <param name="helper"></param>
        public ImageBuilder(HtmlHelper<TModel> helper)
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
            var tagBuilder = new TagBuilder("img");
            var attributes = HtmlAttributesUtility.ObjectToHtmlAttributesDictionary(htmlAttributes);

            attributes.Add("border", "0");
            attributes.Add("src", UrlHelper.GenerateContentUrl(url, _helper.ViewContext.HttpContext));

            tagBuilder.MergeAttributes(attributes);

            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }
    }
}
