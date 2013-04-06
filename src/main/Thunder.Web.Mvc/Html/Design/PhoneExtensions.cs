using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace Thunder.Web.Mvc.Html.Design
{
    /// <summary>
    /// Zipcode extensions
    /// </summary>
    public static class PhoneExtensions
    {
        /// <summary>
        /// Phone
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <param name="name">Name</param>
        /// <returns><see cref="MvcHtmlString"/></returns>
        public static MvcHtmlString Phone(this HtmlHelper helper, string name)
        {
            return helper.Phone(name, "", 14);
            
        }

        /// <summary>
        /// Phone
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <param name="name">Name</param>
        /// <param name="maxLength">Max length</param>
        /// <returns><see cref="MvcHtmlString"/></returns>
        public static MvcHtmlString Phone(this HtmlHelper helper, string name, int maxLength)
        {
            return helper.Phone(name, "", maxLength);
        }

        /// <summary>
        /// Phone
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <param name="name">Name</param>
        /// <param name="value">Value</param>
        /// <param name="maxLength">Max length</param>
        /// <returns><see cref="MvcHtmlString"/></returns>
        public static MvcHtmlString Phone(this HtmlHelper helper, string name, object value, int maxLength)
        {
            return helper.Phone(name, value, maxLength, null);
        }

        /// <summary>
        /// Phone
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <param name="name">Name</param>
        /// <param name="value">Value</param>
        /// <returns><see cref="MvcHtmlString"/></returns>
        public static MvcHtmlString Phone(this HtmlHelper helper, string name, object value)
        {
            return helper.Phone(name, value, 14, null);
        }

        /// <summary>
        /// Phone
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <param name="name">Name</param>
        /// <param name="value">Value</param>
        /// <param name="htmlAttributes">Html attributes</param>
        /// <returns><see cref="MvcHtmlString"/></returns>
        public static MvcHtmlString Phone(this HtmlHelper helper, string name, object value, object htmlAttributes)
        {
            return helper.Phone(name, value, 14, htmlAttributes);
        }

        /// <summary>
        /// Phone
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <param name="name">Name</param>
        /// <param name="value">Value</param>
        /// <param name="maxLength">Max length</param>
        /// <param name="htmlAttributes">Html attributes</param>
        /// <returns><see cref="MvcHtmlString"/></returns>
        public static MvcHtmlString Phone(this HtmlHelper helper, string name, object value, int maxLength, object htmlAttributes)
        {
            var attributes = (htmlAttributes == null ? new RouteValueDictionary() : new RouteValueDictionary(htmlAttributes));

            return helper.Phone(name, value, maxLength, attributes);
        }

        /// <summary>
        /// Phone
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <param name="name">Name</param>
        /// <param name="value">Value</param>
        /// <param name="maxLength">Max length</param>
        /// <param name="htmlAttributes">Html Attributes</param>
        /// <returns><see cref="MvcHtmlString"/></returns>
        public static MvcHtmlString Phone(this HtmlHelper helper, string name, object value, int maxLength, IDictionary<string, object> htmlAttributes)
        {
            var builder = new TagBuilder("input");
            
            builder.MergeAttributes(htmlAttributes);
            
            builder.MergeAttribute("type", "text", true);
            builder.MergeAttribute("name", name, true);
            builder.MergeAttribute("maxlength", maxLength.ToString(CultureInfo.InvariantCulture), true);
            builder.MergeAttribute("value", Convert.ToString(value, CultureInfo.CurrentCulture), true);
            builder.MergeAttribute("class", "text-input phone");

            if (htmlAttributes != null && htmlAttributes.ContainsKey("class"))
            {
                var @class = htmlAttributes.Where(htmlAttribute => htmlAttribute.Key.Equals("class"))
                    .Aggregate("text-input phone", (current, htmlAttribute) => current + (" " + htmlAttribute.Value));

                builder.MergeAttribute("class", @class, true);    
            }
            
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }
    }
}