using System.Web.Mvc;
using System.Web.Routing;

namespace Thunder.Web.Mvc.Html.Design
{
    ///<summary>
    /// Button extensions
    ///</summary>
    public static class ButtonExtensions
    {
        /// <summary>
        /// Button
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <param name="text">Text</param>
        /// <param name="htmlAttributes">Html attributes</param>
        /// <returns>Button</returns>
        public static MvcHtmlString Button(this HtmlHelper helper, string text, object htmlAttributes)
        {
            return helper.Button(text, null, htmlAttributes);
        }

        /// <summary>
        /// Button
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <param name="text">Text</param>
        /// <returns>Button</returns>
        public static MvcHtmlString Button(this HtmlHelper helper, string text)
        {
            return helper.Button(text, null, null);
        }

        /// <summary>
        /// Button
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <param name="text">Text</param>
        /// <param name="title">Title</param>
        /// <returns>Button</returns>
        public static MvcHtmlString Button(this HtmlHelper helper, string text, string title)
        {
            return helper.Button(text, title, null);
        }

        /// <summary>
        /// Button
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <param name="text">Text</param>
        /// <param name="title">Title</param>
        /// <param name="htmlAttributes">Html attributes</param>
        /// <returns>Button</returns>
        public static MvcHtmlString Button(this HtmlHelper helper, string text, string title, object htmlAttributes)
        {
            var builder = new TagBuilder("input");

            builder.Attributes.Add("type", "submit");
            builder.Attributes.Add("value", text);
            builder.Attributes.Add("class", "button");

            if (!string.IsNullOrEmpty(title))
            {
                builder.Attributes.Add("title", title);
            }

            if (htmlAttributes != null)
            {
                builder.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);
            }

            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }
    }
}
