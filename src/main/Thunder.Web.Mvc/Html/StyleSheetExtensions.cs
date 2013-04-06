using System.Web.Mvc;

namespace Thunder.Web.Mvc.Html
{
    /// <summary>
    /// Java style sheet
    /// </summary>
    public static class StyleSheetExtensions
    {
        /// <summary>
        /// Style sheet tag with media screen
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <param name="path">Path</param>
        /// <returns></returns>
        public static MvcHtmlString StyleSheet(this HtmlHelper helper, string path)
        {
            return helper.StyleSheet(path, "screen");
        }

        ///<summary>
        /// Style sheet
        ///</summary>
        ///<param name="helper">Helper</param>
        ///<param name="path">Path</param>
        ///<param name="media">Media</param>
        ///<returns></returns>
        public static MvcHtmlString StyleSheet(this HtmlHelper helper, string path, string media)
        {
            var builder = new TagBuilder("link");

            builder.Attributes.Add("type", "text/css");
            builder.Attributes.Add("media", media);
            builder.Attributes.Add("rel", "stylesheet");
            builder.Attributes.Add("href", UrlHelper.GenerateContentUrl(path, helper.ViewContext.HttpContext));

            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }
    }
}
