using System.IO.Compression;
using System.Web.Mvc;

namespace Thunder.Web.Mvc.Filter
{
    /// <summary>
    /// Compress filter attribute based post http://weblogs.asp.net/rashid/archive/2008/03/28/asp-net-mvc-action-filter-caching-and-compression.aspx.
    /// </summary>
    public class CompressAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// On action executing
        /// </summary>
        /// <param name="filterContext">Action executing context</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var acceptEncoding = request.Headers["Accept-Encoding"];

            if (string.IsNullOrEmpty(acceptEncoding)) return;

            acceptEncoding = acceptEncoding.ToUpperInvariant();

            var response = filterContext.HttpContext.Response;

            if (acceptEncoding.Contains("GZIP"))
            {
                response.AppendHeader("Content-encoding", "gzip");
                response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
            }
            else if (acceptEncoding.Contains("DEFLATE"))
            {
                response.AppendHeader("Content-encoding", "deflate");
                response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
            }
        }
    }
}
