using System;
using System.Web;

namespace Thunder.Web
{
    /// <summary>
    /// Utility
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Converts a virtual path to an application absolute path using the <paramref name="context"/>.
        /// </summary>
        /// <param name="virtualPath">Virtual path</param>
        /// <param name="context"><see cref="HttpContextBase"/></param>
        /// <returns>Absolute path</returns>
        public static string ToAbsolute(string virtualPath, HttpContextBase context)
        {
            return VirtualPathUtility.ToAbsolute(virtualPath, context.Request.ApplicationPath);
        }

        /// <summary>
        /// Converts <paramref name="url"/> to an http absolute address 
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="request"><see cref="HttpRequestBase"/></param>
        /// <returns><see cref="Uri"/></returns>
        public static Uri ToAbsolute(string url, HttpRequestBase request)
        {
            var uri = new Uri(url, UriKind.RelativeOrAbsolute);

            if (uri.IsAbsoluteUri)
                return uri;

            if (request.Url != null)
                url = String.Format("{0}{1}", request.Url.GetLeftPart(UriPartial.Authority), url);

            return new Uri(url);
        }
    }
}