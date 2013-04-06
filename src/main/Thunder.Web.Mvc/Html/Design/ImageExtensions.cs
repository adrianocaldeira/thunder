using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Thunder.Web.Mvc.Html.Design
{
    ///<summary>
    /// Image extensions
    ///</summary>
    public static class ImageExtensions
    {
        ///<summary>
        /// Image
        ///</summary>
        ///<param name="helper">Helper</param>
        ///<param name="path">Path</param>
        ///<param name="htmlAttributes">Html attributes</param>
        ///<returns></returns>
        public static MvcHtmlString Image(this HtmlHelper helper, string path, object htmlAttributes)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            var builder = new TagBuilder("img");

            builder.Attributes.Add("border", "0");
            builder.Attributes.Add("src", UrlHelper.GenerateContentUrl(path, helper.ViewContext.HttpContext));

            if (htmlAttributes != null)
            {
                builder.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);
            }

            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        /// Image
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <param name="path">Path</param>
        /// <returns></returns>
        public static MvcHtmlString Image(this HtmlHelper helper, string path)
        {
            return helper.Image(path, null);
        }

        /// <summary>
        /// Png image
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <param name="path">Path</param>
        /// <returns></returns>
        public static MvcHtmlString Png(this HtmlHelper helper, string path)
        {
            return helper.Png(path, null);
        }

        /// <summary>
        /// Png Image
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <param name="path">Path</param>
        /// <param name="htmlAttributes">Html attributes</param>
        /// <returns></returns>
        public static MvcHtmlString Png(this HtmlHelper helper, string path, object htmlAttributes)
        {
            if(path == null)
                throw new ArgumentNullException("path");

            return helper.Image((path.ToLower().Contains(".png") ? path : path + ".png"), htmlAttributes);
        }

        /// <summary>
        /// Gif image
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <param name="path">Path</param>
        /// <returns></returns>
        public static MvcHtmlString Gif(this HtmlHelper helper, string path)
        {
            return helper.Gif(path, null);
        }

        /// <summary>
        /// Gif Image
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <param name="path">Path</param>
        /// <param name="htmlAttributes">Html attributes</param>
        /// <returns></returns>
        public static MvcHtmlString Gif(this HtmlHelper helper, string path, object htmlAttributes)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            return helper.Image((path.ToLower().Contains(".gif") ? path : path + ".gif"), htmlAttributes);
        }

        /// <summary>
        /// Jpg image
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <param name="path">Path</param>
        /// <returns></returns>
        public static MvcHtmlString Jpg(this HtmlHelper helper, string path)
        {
            return helper.Jpg(path, null);
        }

        /// <summary>
        /// Jpg Image
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <param name="path">Path</param>
        /// <param name="htmlAttributes">Html attributes</param>
        /// <returns></returns>
        public static MvcHtmlString Jpg(this HtmlHelper helper, string path, object htmlAttributes)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            return helper.Image((path.ToLower().Contains(".jpg") ? path : path + ".jpg"), htmlAttributes);
        }

    }
}
