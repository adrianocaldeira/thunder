using System;
using System.Linq;
using System.Web.Mvc;
using Thunder.Data;

namespace Thunder.Web.Mvc
{
    /// <summary>
    /// NHibernate session per request attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class SessionPerRequestAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Get or set exclude action with comma separate
        /// </summary>
        public string Exclude { get; set; }

        /// <summary>
        /// OnActionExecuting
        /// </summary>
        /// <param name="filterContext"><see cref="ActionExecutingContext"/></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Apply(filterContext.ActionDescriptor.ActionName))
            {
                SessionManager.Bind();
            }
        }

        /// <summary>
        /// OnResultExecuted
        /// </summary>
        /// <param name="filterContext"><see cref="ResultExecutedContext"/></param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (Apply(filterContext.RequestContext.RouteData.GetRequiredString("action")))
            {
                SessionManager.Unbind();
            }
        }

        /// <summary>
        /// Check if action is exclud
        /// </summary>
        /// <param name="action">Action name</param>
        /// <returns>Apply</returns>
        private bool Apply(string action)
        {
            return string.IsNullOrEmpty(Exclude) || Exclude.Split(',').All(item => !item.ToLower().Equals(action.ToLower()));
        }
    }
}
