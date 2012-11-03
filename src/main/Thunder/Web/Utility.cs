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
    }
}