using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Manager.Library;
using Manager.Models;
using Thunder.Web;
using JsonResult = Thunder.Web.Mvc.JsonResult;

namespace Manager.Filters
{
    public class AuthorizedAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Urls ignoradas
        /// </summary>
        public IList<string> IgnoreUrls
        {
            get { return new List<string> {"/login"}; }
        }

        /// <summary>
        /// OnActionExecuting
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //if (filterContext.HttpContext.Session != null)
            //{
            //    var funcionalities = filterContext.HttpContext.Session[HardCode.Session.Functionalities] as IList<Functionality>;

            //    if (funcionalities != null && funcionalities.Count > 0)
            //    {
            //        CheckAuthorization(filterContext, funcionalities);
            //    }
            //    else
            //    {
            //        NotConnected(filterContext);
            //    }
            //}
        }

        //private static string UrlLogin(HttpContextBase context)
        //{
        //    var redirectUrl = HttpUtility.UrlEncode(context.Request.Url.AbsolutePath);

        //    return Url(string.Format("~/login?path={0}", redirectUrl), context);
        //}

        //private static string Url(string virtualPath, HttpContextBase context)
        //{
        //    return VirtualPathUtility.ToAbsolute(virtualPath, context.Request.ApplicationPath);
        //}

        //private static void NotConnected(ActionExecutingContext filterContext)
        //{
        //    filterContext.Result = new RedirectResult(UrlLogin(filterContext.HttpContext));

        //    if (filterContext.HttpContext.Request.IsAjaxRequest())
        //    {
        //        filterContext.Result = new JsonResult(ResultStatus.NotConnected)
        //        {
        //            Data = new { Url = UrlLogin(filterContext.HttpContext) }
        //        };
        //    }
        //}

        //private void CheckAuthorization(ActionExecutingContext filterContext, IEnumerable<Functionality> funcionalities)
        //{
        //    if (filterContext.HttpContext.Request.Url != null)
        //    {
        //        var url = filterContext.HttpContext.Request.Url.AbsolutePath;

        //        if (!IsIgnoreUrl(url) && !AllowAccess(url, funcionalities, filterContext.HttpContext))
        //        {
        //            filterContext.Result = new HttpUnauthorizedResult();
        //        }
        //    }
        //}

        //private bool IsIgnoreUrl(string url)
        //{
        //    return IgnoreUrls.Any(ignoreUrl => url.Contains(url));
        //}

        //private static bool AllowAccess(string url, IEnumerable<Functionality> funcionalities,
        //                                HttpContextBase httpContext)
        //{
        //    foreach (var functionality in funcionalities)
        //    {
        //        if ((!string.IsNullOrEmpty(functionality.Path) && url.Contains(Url(functionality.Path, httpContext))))
        //        {
        //            return true;
        //        }

        //        if (functionality.Childs.Any(child => (!string.IsNullOrEmpty(child.Path) && url.Contains(Url(child.Path, httpContext)))))
        //        {
        //            return true;
        //        }
        //    }

        //    return false;
        //}
    }
}