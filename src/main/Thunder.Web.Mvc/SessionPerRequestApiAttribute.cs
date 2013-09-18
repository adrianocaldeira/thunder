using System;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Thunder.Data;

namespace Thunder.Web.Mvc
{
    /// <summary>
    /// NHibernate session per request attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class SessionPerRequestApiAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Get or set exclude action with comma separate
        /// </summary>
        public string Exclude { get; set; }

        /// <summary>
        /// OnActionExecuting
        /// </summary>
        /// <param name="actionContext"><see cref="HttpActionContext"/></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (Apply(actionContext.ActionDescriptor.ActionName))
            {
                SessionManager.Bind();
            }
        }

        /// <summary>
        /// OnActionExecuted
        /// </summary>
        /// <param name="actionExecutedContext"><see cref="HttpActionExecutedContext"/></param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (Apply(actionExecutedContext.ActionContext.ActionDescriptor.ActionName))
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