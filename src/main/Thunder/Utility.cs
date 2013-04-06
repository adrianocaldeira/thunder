using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace Thunder
{
    /// <summary>
    /// Utility
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Get property name
        /// </summary>
        /// <param name="expression">Expression</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>Name</returns>
        public static string GetPropertyName<T>(Expression<Func<T, Object>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;

            if (memberExpression != null)
            {
                return memberExpression.Member.Name;
            }
            
            var operand = ((UnaryExpression)expression.Body).Operand;
            
            return ((MemberExpression)operand).Member.Name;
        }

        /// <summary>
        /// Converts a virtual path to an application absolute path using the <paramref name="context"/>.
        /// </summary>
        /// <param name="virtualPath">Virtual path</param>
        /// <param name="context"><see cref="HttpContextBase"/></param>
        /// <returns>Absolute path</returns>
        public static string ToAbsolute(string virtualPath, HttpContextBase context)
        {
            return VirtualPathUtility.ToAbsolute(virtualPath, context.Request.ApplicationPath);
        }

        /// <summary>
        /// Converts <paramref name="url"/> to an absolute web address
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="request"><see cref="HttpRequestBase"/></param>
        /// <returns><see cref="Uri"/></returns>
        public static Uri ToAbsolute(string url, HttpRequestBase request)
        {
            var uri = new Uri(url, UriKind.RelativeOrAbsolute);

            if (uri.IsAbsoluteUri)
                return uri;

            if (request.Url != null)
                url = String.Format("{0}{1}", request.Url.GetLeftPart(UriPartial.Authority), url);

            return new Uri(url);
        }
    }
}
