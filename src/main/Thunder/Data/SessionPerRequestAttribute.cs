using System;
using System.Web.Mvc;

namespace Thunder.Data
{
    /// <summary>
    /// NHibernate session per request attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class SessionPerRequestAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// OnActionExecuting
        /// </summary>
        /// <param name="filterContext"><see cref="ActionExecutingContext"/></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SessionManager.Bind();
        }

        /// <summary>
        /// OnActionExecuted
        /// </summary>
        /// <param name="filterContext"><see cref="ActionExecutedContext"/></param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            SessionManager.Unbind();
        }
    }
}
