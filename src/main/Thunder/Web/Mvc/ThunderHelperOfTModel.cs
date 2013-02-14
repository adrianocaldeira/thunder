using System.Web.Mvc;
using System.Web.Routing;

namespace Thunder.Web.Mvc
{
    /// <summary>
    /// Thunder helper
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class ThunderHelper<TModel> : ThunderHelper
    {
        /// <summary>
        /// Initialize new instance of <see cref="ThunderHelper{TModel}"/>
        /// </summary>
        /// <param name="viewContext"><see cref="ViewContext"/></param>
        /// <param name="viewDataContainer"><see cref="IViewDataContainer"/></param>
        /// <param name="htmlHelper"><see cref="HtmlHelper"/></param>
        public ThunderHelper(ViewContext viewContext, IViewDataContainer viewDataContainer, HtmlHelper htmlHelper) :
            this(viewContext, viewDataContainer, RouteTable.Routes, htmlHelper)
        {
        }

        /// <summary>
        /// Initialize new instance of <see cref="ThunderHelper{TModel}"/>
        /// </summary>
        /// <param name="viewContext"><see cref="ViewContext"/></param>
        /// <param name="viewDataContainer"><see cref="IViewDataContainer"/></param>
        /// <param name="routeCollection"><see cref="RouteCollection"/></param>
        /// <param name="htmlHelper"><see cref="HtmlHelper"/></param>
        public ThunderHelper(ViewContext viewContext, IViewDataContainer viewDataContainer, RouteCollection routeCollection, HtmlHelper htmlHelper) : 
            base(viewContext, viewDataContainer, routeCollection, htmlHelper)
        {
            Html = (HtmlHelper<TModel>) htmlHelper;
        }

        /// <summary>
        /// Get html helper
        /// </summary>
        public new HtmlHelper<TModel> Html { get; private set; }
    }
}
