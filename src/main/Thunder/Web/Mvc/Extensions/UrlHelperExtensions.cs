using System;
using System.Web.Mvc;

namespace Thunder.Web.Mvc.Extensions
{
    /// <summary>
    /// Url helper extensions
    /// </summary>
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Get absolute url
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <param name="url">Url</param>
        /// <returns>Absolute url</returns>
        public static Uri Absolute(this UrlHelper helper, string url)
        {
            var uri = new Uri(url, UriKind.RelativeOrAbsolute);
            if (uri.IsAbsoluteUri)
                return uri;

            if (helper.RequestContext.HttpContext.Request.Url != null)
                url = String.Format("{0}{1}", helper.RequestContext.HttpContext.Request.Url.GetLeftPart(UriPartial.Authority), url);

            return new Uri(url);
        }
    }
}
