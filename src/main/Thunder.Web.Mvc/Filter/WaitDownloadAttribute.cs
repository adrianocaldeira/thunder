using System;
using System.Web;
using System.Web.Mvc;

namespace Thunder.Web.Mvc.Filter
{
    /// <summary>
    /// Wait download attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class WaitDownloadAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Initiliza new instance of <see cref="WaitDownloadAttribute"/>
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="cookiePath"></param>
        public WaitDownloadAttribute(string cookieName = "fileDownload", string cookiePath = "/")
        {
            CookieName = cookieName;
            CookiePath = cookiePath;
        }

        /// <summary>
        /// Get or set CookieName
        /// </summary>
        public string CookieName { get; set; }

        /// <summary>
        /// Get or set CookiePath
        /// </summary>
        public string CookiePath { get; set; }

        /// <summary>
        /// If the current response is a FileResult (an MVC base class for files) then write a
        /// cookie to inform jquery.fileDownload that a successful file download has occured
        /// </summary>
        /// <param name="filterContext"></param>
        private void CheckAndHandleFileResult(ActionExecutedContext filterContext)
        {
            var httpContext = filterContext.HttpContext;
            var response = httpContext.Response;

            if (filterContext.Result is FileResult)
                //jquery.fileDownload uses this cookie to determine that a file download has completed successfully
                response.AppendCookie(new HttpCookie(CookieName, "true") { Path = CookiePath, HttpOnly = false });
            else
                //ensure that the cookie is removed in case someone did a file download without using jquery.fileDownload
            if (httpContext.Request.Cookies[CookieName] != null)
            {
                response.AppendCookie(new HttpCookie(CookieName, "true") { Expires = DateTime.Now.AddYears(-1), Path = CookiePath });
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            CheckAndHandleFileResult(filterContext);

            base.OnActionExecuted(filterContext);
        }
    }
}
