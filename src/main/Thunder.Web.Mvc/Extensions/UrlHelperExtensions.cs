using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Thunder.Web.Mvc.Internal;

namespace Thunder.Web.Mvc.Extensions
{
    /// <summary>
    /// Simple extension methods for UrlHelper to always generate absolute urls. Copy from https://bitbucket.org/swaj/actionmailer.net/raw/80075ee3aaf60067bc5f47faf2aabffaa4b6cfa5/src/ActionMailer.Net.Mvc/UrlHelperExtensions.cs
    /// </summary>
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Generates an absolute url for the provided action.
        /// </summary>
        /// <param name="helper">The UrlHelper instance to use for generation.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <returns>An absolute url.</returns>
        public static string AbsoluteAction(this UrlHelper helper, string actionName)
        {
            var url = helper.Action(actionName);
            return GetAbsoluteUrl(helper.RequestContext.HttpContext.Request, url).ToString();
        }

        /// <summary>
        /// Generates an absolute url for the provided action.
        /// </summary>
        /// <param name="helper">The UrlHelper instance to use for generation.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <returns>An absolute url.</returns>
        public static string AbsoluteAction(this UrlHelper helper, string actionName, string controllerName)
        {
            var url = helper.Action(actionName, controllerName);
            return GetAbsoluteUrl(helper.RequestContext.HttpContext.Request, url).ToString();
        }

        /// <summary>
        /// Generates an absolute url for the provided action.
        /// </summary>
        /// <param name="helper">The UrlHelper instance to use for generation.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="routeValues">Any extra route values you wish to use for generation.</param>
        /// <returns>An absolute url.</returns>
        public static string AbsoluteAction(this UrlHelper helper, string actionName, object routeValues)
        {
            var url = helper.Action(actionName, routeValues);
            return GetAbsoluteUrl(helper.RequestContext.HttpContext.Request, url).ToString();
        }

        /// <summary>
        /// Generates an absolute url for the provided action.
        /// </summary>
        /// <param name="helper">The UrlHelper instance to use for generation.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="routeValues">Any extra route values you wish to use for generation.</param>
        /// <returns>An absolute url.</returns>
        public static string AbsoluteAction(this UrlHelper helper, string actionName, RouteValueDictionary routeValues)
        {
            var url = helper.Action(actionName, routeValues);
            return GetAbsoluteUrl(helper.RequestContext.HttpContext.Request, url).ToString();
        }

        /// <summary>
        /// Generates an absolute url for the provided action.
        /// </summary>
        /// <param name="helper">The UrlHelper instance to use for generation.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">Any extra route values you wish to use for generation.</param>
        /// <returns>An absolute url.</returns>
        public static string AbsoluteAction(this UrlHelper helper, string actionName, string controllerName, object routeValues)
        {
            var url = helper.Action(actionName, controllerName, routeValues);
            return GetAbsoluteUrl(helper.RequestContext.HttpContext.Request, url).ToString();
        }

        /// <summary>
        /// Generates an absolute url for the provided action.
        /// </summary>
        /// <param name="helper">The UrlHelper instance to use for generation.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">Any extra route values you wish to use for generation.</param>
        /// <returns>An absolute url.</returns>
        public static string AbsoluteAction(this UrlHelper helper, string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            var url = helper.Action(actionName, controllerName, routeValues);
            return GetAbsoluteUrl(helper.RequestContext.HttpContext.Request, url).ToString();
        }

        /// <summary>
        /// Generates an absolute url for the provided action.
        /// </summary>
        /// <param name="helper">The UrlHelper instance to use for generation.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">Any extra route values you wish to use for generation.</param>
        /// <param name="protocol">The protocol to use (http or https usually).</param>
        /// <returns>An absolute url.</returns>
        public static string AbsoluteAction(this UrlHelper helper, string actionName, string controllerName, object routeValues, string protocol)
        {
            var url = helper.Action(actionName, controllerName, routeValues, protocol);
            return GetAbsoluteUrl(helper.RequestContext.HttpContext.Request, url).ToString();
        }

        /// <summary>
        /// Generates an absolute url for the provided action.
        /// </summary>
        /// <param name="helper">The UrlHelper instance to use for generation.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">Any extra route values you wish to use for generation.</param>
        /// <param name="protocol">The protocol to use (http or https usually).</param>
        /// <param name="hostName">The host name to use.</param>
        /// <returns>An absolute url.</returns>
        public static string AbsoluteAction(this UrlHelper helper, string actionName, string controllerName, RouteValueDictionary routeValues, string protocol, string hostName)
        {
            var url = helper.Action(actionName, controllerName, routeValues, protocol, hostName);
            return GetAbsoluteUrl(helper.RequestContext.HttpContext.Request, url).ToString();
        }

        /// <summary>
        /// Generates an absolute url for the provided route.
        /// </summary>
        /// <param name="helper">The UrlHelper instance to use for generation.</param>
        /// <param name="routeValues">Any extra route values you wish to use for generation.</param>
        /// <returns></returns>
        public static string AbsoluteRouteUrl(this UrlHelper helper, object routeValues)
        {
            var url = helper.RouteUrl(routeValues);
            return GetAbsoluteUrl(helper.RequestContext.HttpContext.Request, url).ToString();
        }

        /// <summary>
        /// Generates an absolute url for the provided route.
        /// </summary>
        /// <param name="helper">The UrlHelper instance to use for generation.</param>
        /// <param name="routeName">The name of the route to use.</param>
        /// <returns>An absolute url.</returns>
        public static string AbsoluteRouteUrl(this UrlHelper helper, string routeName)
        {
            var url = helper.RouteUrl(routeName);
            return GetAbsoluteUrl(helper.RequestContext.HttpContext.Request, url).ToString();
        }

        /// <summary>
        /// Generates an absolute url for the provided route.
        /// </summary>
        /// <param name="helper">The UrlHelper instance to use for generation.</param>
        /// <param name="routeValues">Any extra route values you wish to use for generation.</param>
        /// <returns>An absolute url.</returns>
        public static string AbsoluteRouteUrl(this UrlHelper helper, RouteValueDictionary routeValues)
        {
            var url = helper.RouteUrl(routeValues);
            return GetAbsoluteUrl(helper.RequestContext.HttpContext.Request, url).ToString();
        }

        /// <summary>
        /// Generates an absolute url for the provided route.
        /// </summary>
        /// <param name="helper">The UrlHelper instance to use for generation.</param>
        /// <param name="routeName">The name of the route to use.</param>
        /// <param name="routeValues">Any extra route values you wish to use for generation.</param>
        /// <returns>An absolute url.</returns>
        public static string AbsoluteRouteUrl(this UrlHelper helper, string routeName, object routeValues)
        {
            var url = helper.RouteUrl(routeName, routeValues);
            return GetAbsoluteUrl(helper.RequestContext.HttpContext.Request, url).ToString();
        }

        /// <summary>
        /// Generates an absolute url for the provided route.
        /// </summary>
        /// <param name="helper">The UrlHelper instance to use for generation.</param>
        /// <param name="routeName">The name of the route to use.</param>
        /// <param name="routeValues">Any extra route values you wish to use for generation.</param>
        /// <returns>An absolute url.</returns>
        public static string AbsoluteRouteUrl(this UrlHelper helper, string routeName, RouteValueDictionary routeValues)
        {
            var url = helper.RouteUrl(routeName, routeValues);
            return GetAbsoluteUrl(helper.RequestContext.HttpContext.Request, url).ToString();
        }

        /// <summary>
        /// Generates an absolute url for the provided route.
        /// </summary>
        /// <param name="helper">The UrlHelper instance to use for generation.</param>
        /// <param name="routeName">The name of the route to use.</param>
        /// <param name="routeValues">Any extra route values you wish to use for generation.</param>
        /// <param name="protocol">The protocol to use (http or https usually).</param>
        /// <returns>An absolute url.</returns>
        public static string AbsoluteRouteUrl(this UrlHelper helper, string routeName, object routeValues, string protocol)
        {
            var url = helper.RouteUrl(routeName, routeValues, protocol);
            return GetAbsoluteUrl(helper.RequestContext.HttpContext.Request, url).ToString();
        }

        /// <summary>
        /// Generates an absolute url for the provided virtual content path.
        /// </summary>
        /// <param name="helper">The UrlHelper instance to use for generation.</param>
        /// <param name="contentPath">The virtual path of the content.</param>
        /// <returns></returns>
        public static string AbsoluteContent(this UrlHelper helper, string contentPath)
        {
            var url = helper.Content(contentPath);
            return GetAbsoluteUrl(helper.RequestContext.HttpContext.Request, url).ToString();
        }

        /// <summary>
        /// Generates an absolute url for the provided route.
        /// </summary>
        /// <param name="helper">The UrlHelper instance to use for generation.</param>
        /// <param name="routeName">The name of the route to use.</param>
        /// <param name="routeValues">Any extra route values you wish to use for generation.</param>
        /// <param name="protocol">The protocol to use (http or https usually).</param>
        /// <param name="hostName">The host name to use.</param>
        /// <returns>An absolute url.</returns>
        public static string AbsoluteRouteUrl(this UrlHelper helper, string routeName, RouteValueDictionary routeValues, string protocol, string hostName)
        {
            var url = helper.RouteUrl(routeName, routeValues, protocol, hostName);
            return GetAbsoluteUrl(helper.RequestContext.HttpContext.Request, url).ToString();
        }

        private static Uri GetAbsoluteUrl(HttpRequestBase request, string url)
        {
            var uri = new Uri(url, UriKind.RelativeOrAbsolute);
            if (uri.IsAbsoluteUri)
                return uri;

            url = ComposeAbsoluteUrl(CanonicalHost.Value, request.Url.Scheme, request.Url.GetLeftPart(UriPartial.Authority), url);
            return new Uri(url);
        }

        /// <summary>
        /// Composes an absolute URL by prefixing <paramref name="relativeUrl"/> with an authority
        /// (scheme + host [+ port]).
        /// </summary>
        /// <remarks>
        /// Mitigates host header poisoning (A4): when <paramref name="canonicalHost"/> is <c>null</c>
        /// (the default, opt-in-off state), the authority is taken from <paramref name="requestAuthority"/>
        /// — this preserves the pre-existing behavior of trusting the incoming request's Host header.
        /// When <paramref name="canonicalHost"/> is configured, it is used instead: if it already
        /// contains a scheme (i.e. contains "://"), it is used verbatim as the full authority; otherwise
        /// it is treated as a bare host and prefixed with <paramref name="scheme"/> (the request's own
        /// scheme is preserved so http vs https is not silently changed).
        /// </remarks>
        /// <param name="canonicalHost">The configured canonical host (<see cref="CanonicalHost.Value"/>), or <c>null</c> when not configured.</param>
        /// <param name="scheme">The request's scheme (e.g. "http" or "https"), used when <paramref name="canonicalHost"/> has no scheme of its own.</param>
        /// <param name="requestAuthority">The request's own authority (e.g. "https://host"), used when <paramref name="canonicalHost"/> is <c>null</c>.</param>
        /// <param name="relativeUrl">The relative URL to append to the resolved authority.</param>
        /// <returns>The composed absolute URL string.</returns>
        internal static string ComposeAbsoluteUrl(string canonicalHost, string scheme, string requestAuthority, string relativeUrl)
        {
            if (canonicalHost == null)
                return requestAuthority + relativeUrl;

            var authority = canonicalHost.IndexOf("://", StringComparison.Ordinal) >= 0
                ? canonicalHost
                : scheme + Uri.SchemeDelimiter + canonicalHost;

            return authority + relativeUrl;
        }
    }
}
