using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Manager.Library;
using Manager.Models;
using Thunder.Web;
using Thunder.Web.Mvc;
using JsonResult = Thunder.Web.Mvc.JsonResult;

namespace Manager.Filters
{
    public class AuthorizedAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var context = filterContext.HttpContext;
            var user = context.Session[HardCode.Session.ConnectedUser] as User;

            if (user == null)
            {
                var route = new RouteValueDictionary
                    {
                        {"Action", "Index"}, {"Controller", "Login"}, {"ReturnUrl", context.Request.RawUrl}
                    };

                if (context.Request.IsAjaxRequest())
                {
                    filterContext.Result = new JsonResult
                    {
                        Status = ResultStatus.NotConnected,
                        Data = new { Url = RouteTable.Routes.GetVirtualPath(filterContext.RequestContext, route).VirtualPath }
                    };
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(route);
                }
            }
            else
            {
                var httpMethod = filterContext.RequestContext.HttpContext.Request.HttpMethod;
                var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                var actionName = filterContext.ActionDescriptor.ActionName;

                if (NeedCheckAcces(controllerName, actionName) && !User.AllowAccess(user.Id, controllerName, actionName, httpMethod))
                {
                    filterContext.Result = new HttpUnauthorizedResult();
                }
            }
        }

        private static bool NeedCheckAcces(string controllerName, string actionName)
        {
            var dictionary = new Dictionary<string, string> { { "Home", "Index" } };

            return dictionary.All(item => !item.Key.ToLower().Equals(controllerName.ToLower()) || !item.Value.ToLower().Equals(actionName.ToLower()));
        }
    }
}