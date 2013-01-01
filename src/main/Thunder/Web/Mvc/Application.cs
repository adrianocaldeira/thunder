using System.Web.Mvc;
using System.Web.Routing;
using log4net.Config;

namespace Thunder.Web.Mvc
{
    /// <summary>
    /// Mvc application
    /// </summary>
    public class Application :  System.Web.HttpApplication
    {
        /// <summary>
        /// Register global filters
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="routes">Routes</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.Clear();

            routes.RouteExistingFiles = true;

            routes.IgnoreRoute("{file}.txt");
            routes.IgnoreRoute("{file}.htm");
            routes.IgnoreRoute("{file}.html");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("content/{*pathInfo}");
            routes.IgnoreRoute("css/{*pathInfo}");
            routes.IgnoreRoute("images/{*pathInfo}");
            routes.IgnoreRoute("scripts/{*pathInfo}");
            routes.IgnoreRoute("cache/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.([iI][cC][oO]|[gG][iI][fF])(/.*)?" });
            
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        /// <summary>
        /// Application start
        ///  </summary>
        protected void Application_Start()
        {
            XmlConfigurator.Configure();

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}
