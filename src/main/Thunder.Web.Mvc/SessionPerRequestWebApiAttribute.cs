using System;
using System.Linq;
using System.Web.Http.Filters;
using Thunder.Data;

namespace Thunder.Web.Mvc
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class SessionPerRequestWebApi : ActionFilterAttribute
    {
        public string Exclude { get; set; }
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);

            if (Apply(actionContext.ActionDescriptor.ActionName))
            {

                SessionManager.Bind();

            }

        }
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
            if (Apply(actionExecutedContext.ActionContext.ActionDescriptor.ActionName))
            {

                SessionManager.Unbind();
            }
        }

        private bool Apply(string action)
        {
            return string.IsNullOrEmpty(Exclude) || Exclude.Split(',').All(item => !item.ToLower().Equals(action.ToLower()));
        }
    }
}
