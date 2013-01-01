using System.Web.Mvc;

namespace Thunder.Web.Mvc.Html
{
    /// <summary>
    /// Java script helper
    /// </summary>
    public static class JavaScriptExtensions
    {
        /// <summary>
        /// Java script tag
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <param name="path">Path</param>
        /// <returns></returns>
        public static MvcHtmlString JavaScript(this HtmlHelper helper, string path)
        {
            var builder = new TagBuilder("script");

            builder.Attributes.Add("type", "text/javascript");
            builder.Attributes.Add("src", UrlHelper.GenerateContentUrl(path, helper.ViewContext.HttpContext));

            return MvcHtmlString.Create(builder.ToString());
        }
    }
}
