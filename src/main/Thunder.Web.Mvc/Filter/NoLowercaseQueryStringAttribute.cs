using System;
using System.Web.Mvc;

namespace Thunder.Web.Mvc.Filter
{
    /// <summary>
    /// Ensures that a HTTP request URL can contain query string parameters with both upper-case and lower-case
    /// characters.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class NoLowercaseQueryStringAttribute : FilterAttribute
    {
    }
}
