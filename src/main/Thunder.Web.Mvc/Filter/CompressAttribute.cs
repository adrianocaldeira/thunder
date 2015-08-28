using System.IO.Compression;
using System.Web.Mvc;

namespace Thunder.Web.Mvc.Filter
{
    /// <summary>
    /// Compress filter attribute 
    /// </summary>
    public class CompressAttribute : ActionFilterAttribute
    {
        private static bool IsGZipSupported(ControllerContext filterContext)
        {
            var acceptEncoding = filterContext.RequestContext.HttpContext.Request.Headers["Accept-Encoding"];

            return !string.IsNullOrEmpty(acceptEncoding) && (acceptEncoding.ToLower().Contains("gzip") || acceptEncoding.ToLower().Contains("deflate"));
        }

        /// <summary>
        /// On action executing
        /// </summary>
        /// <param name="filterContext">Action executing context</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var response = filterContext.HttpContext.Response;

            if (response.Filter == null) return;

            if(IsGZipSupported(filterContext))
            {
                var acceptEncoding = request.Headers["Accept-Encoding"].ToLower();

                if (acceptEncoding.Contains("gzip"))
                {
                    response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
                    response.Headers.Remove("Content-Encoding");
                    response.AppendHeader("Content-Encoding", "gzip");
                }
                else
                {
                    response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
                    response.Headers.Remove("Content-Encoding");
                    response.AppendHeader("Content-Encoding", "deflate");
                }
            }
            
            response.AppendHeader("Vary", "Content-Encoding");
        }
    }
}
