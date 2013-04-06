using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace Thunder.Web.Mvc.Html.Design
{
    /// <summary>
    /// Numeric extensions
    /// </summary>
    public static class NumericNExtensions
    {
        /// <summary>
        /// TextBox
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <param name="name">Name</param>
        /// <param name="maxLength">Max length</param>
        /// <returns><see cref="MvcHtmlString"/></returns>
        public static MvcHtmlString Numeric(this HtmlHelper helper, string name, int maxLength)
        {
            return helper.Numeric(name, "", maxLength);
        }

        /// <summary>
        /// Numeric
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <param name="name">Name</param>
        /// <param name="value">Value</param>
        /// <param name="maxLength">Max length</param>
        /// <returns><see cref="MvcHtmlString"/></returns>
        public static MvcHtmlString Numeric(this HtmlHelper helper, string name, object value, int maxLength)
        {
            return helper.Numeric(name, value, maxLength, null);
        }

        /// <summary>
        /// Numeric
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <param name="name">Name</param>
        /// <param name="value">Value</param>
        /// <param name="maxLength">Max length</param>
        /// <param name="htmlAttributes">Html attributes</param>
        /// <returns><see cref="MvcHtmlString"/></returns>
        public static MvcHtmlString Numeric(this HtmlHelper helper, string name, object value, int maxLength,
                                            object htmlAttributes)
        {
            RouteValueDictionary attributes = (htmlAttributes == null
                                                   ? new RouteValueDictionary()
                                                   : new RouteValueDictionary(htmlAttributes));

            return helper.Numeric(name, value, maxLength, attributes);
        }

        /// <summary>
        /// Numeric
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <param name="name">Name</param>
        /// <param name="value">Value</param>
        /// <param name="maxLength">Max length</param>
        /// <param name="htmlAttributes">Html Attributes</param>
        /// <returns><see cref="MvcHtmlString"/></returns>
        public static MvcHtmlString Numeric(this HtmlHelper helper, string name, object value, int maxLength,
                                            IDictionary<string, object> htmlAttributes)
        {
            var builder = new TagBuilder("input");

            builder.MergeAttributes(htmlAttributes);

            builder.MergeAttribute("type", "text", true);
            builder.MergeAttribute("name", name, true);
            builder.MergeAttribute("maxlength", maxLength.ToString(CultureInfo.InvariantCulture), true);
            builder.MergeAttribute("value", Convert.ToString(value, CultureInfo.CurrentCulture), true);
            builder.MergeAttribute("class", "text-input numeric");

            if (htmlAttributes != null && htmlAttributes.ContainsKey("class"))
            {
                var @class = htmlAttributes.Where(htmlAttribute => htmlAttribute.Key.Equals("class"))
                    .Aggregate("text-input numeric", (current, htmlAttribute) => current + (" " + htmlAttribute.Value));

                builder.MergeAttribute("class", @class, true);
            }

            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }
    }
}