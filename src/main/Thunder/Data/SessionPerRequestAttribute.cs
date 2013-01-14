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
        /// OnResultExecuted
        /// </summary>
        /// <param name="filterContext"><see cref="ResultExecutedContext"/></param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            SessionManager.Unbind();
        }
    }
}
