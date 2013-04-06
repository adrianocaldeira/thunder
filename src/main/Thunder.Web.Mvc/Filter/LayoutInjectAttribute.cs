using System.Web.Mvc;

namespace Thunder.Web.Mvc.Filter
{
    /// <summary>
    /// Layout inject
    /// </summary>
    public class LayoutInjectAttribute : ActionFilterAttribute
    {
        private readonly string _masterView;

        /// <summary>
        /// Initialize new instance of <see cref="LayoutInjectAttribute"/>.
        /// </summary>
        /// <param name="masterView">Master view</param>
        public LayoutInjectAttribute(string masterView)
        {
            _masterView = masterView;
        }

        /// <summary>
        /// Called by the ASP.NET MVC framework after the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            var result = filterContext.Result as ViewResult;
            if (result == null) return;

            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest() || result.ViewName.Contains("_"))
            {
                result.MasterName = null;
            }
            else
            {
                result.MasterName = _masterView;
            }
        }
    }
}
