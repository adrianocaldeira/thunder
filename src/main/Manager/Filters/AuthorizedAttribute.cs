using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Manager.Library;
using Manager.Models;

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
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                        {
                            {"Action", "Index"},
                            {"Controller", "Login"},
                            {"ReturnUrl", context.Request.RawUrl}
                        }
                    );
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
            var dictionary = new Dictionary<string, string> {{"Home", "Index"}};

            return dictionary.All(item => !item.Key.ToLower().Equals(controllerName.ToLower()) || !item.Value.ToLower().Equals(actionName.ToLower()));
        }
    }
}