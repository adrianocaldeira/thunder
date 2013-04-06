using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Thunder.Web.Mvc
{
    /// <summary>
    /// Thunder helper
    /// </summary>
    public class ThunderHelper
    {
        /// <summary>
        /// Initialize new instance of class <see cref="ThunderHelper"/>.
        /// </summary>
        /// <param name="viewContext"><see cref="ViewContext"/></param>
        /// <param name="viewDataContainer"><see cref="IViewDataContainer"/></param>
        /// <param name="htmlHelper"><see cref="HtmlHelper"/></param>
        public ThunderHelper(ViewContext viewContext, IViewDataContainer viewDataContainer, HtmlHelper htmlHelper) :
            this(viewContext, viewDataContainer, RouteTable.Routes, htmlHelper)
        {
        }

        /// <summary>
        /// Initialize new instance of class <see cref="ThunderHelper"/>.
        /// </summary>
        /// <param name="viewContext"><see cref="ViewContext"/></param>
        /// <param name="viewDataContainer"><see cref="IViewDataContainer"/></param>
        /// <param name="routeCollection"><see cref="RouteCollection"/></param>
        /// <param name="htmlHelper"><see cref="HtmlHelper"/></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ThunderHelper(ViewContext viewContext, IViewDataContainer viewDataContainer,
                             RouteCollection routeCollection, HtmlHelper htmlHelper)
        {
            if (viewContext == null)
            {
                throw new ArgumentNullException("viewContext");
            }

            if (viewDataContainer == null)
            {
                throw new ArgumentNullException("viewDataContainer");
            }

            if (routeCollection == null)
            {
                throw new ArgumentNullException("routeCollection");
            }

            ViewContext = viewContext;
            RouteCollection = routeCollection;
            Html = htmlHelper;
            ViewData = new ViewDataDictionary(viewDataContainer.ViewData);
        }

        /// <summary>
        /// Get route collection
        /// </summary>
        public RouteCollection RouteCollection { get; private set; }

        /// <summary>
        /// Get view context
        /// </summary>
        public ViewContext ViewContext { get; private set; }

        /// <summary>
        /// Get html helper
        /// </summary>
        public HtmlHelper Html { get; private set; }

        /// <summary>
        /// Get view data
        /// </summary>
        public ViewDataDictionary ViewData { get; private set; }
    }
}