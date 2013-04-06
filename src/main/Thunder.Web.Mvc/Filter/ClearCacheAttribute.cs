using System;
using System.Web;
using System.Web.Mvc;

namespace Thunder.Web.Mvc.Filter
{
    /// <summary>
    /// Clear http cache
    /// </summary>
    public class ClearCacheAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// On action executing
        /// </summary>
        /// <param name="filterContext">Filer context</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
            filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            filterContext.HttpContext.Response.Cache.SetNoStore();
        }
    }
}
