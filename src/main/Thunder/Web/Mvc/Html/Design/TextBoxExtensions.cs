using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace Thunder.Web.Mvc.Html.Design
{
    /// <summary>
    /// Text box extensions
    /// </summary>
    public static class TextBoxExtensions
    {
        /// <summary>
        /// TextBox
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <param name="name">Name</param>
        /// <param name="maxLength">Maxlength</param>
        /// <param name="widthStyle">Width style</param>
        /// <returns>TextBox</returns>
        public static MvcHtmlString TextBox(this HtmlHelper helper, string name, int maxLength, WidthStyle widthStyle)
        {
            return helper.TextBox(name, maxLength, widthStyle, null);
        }

        /// <summary>
        /// TextBox
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <param name="name">Name</param>
        /// <param name="maxLength">Maxlength</param>
        /// <param name="widthStyle">Width style</param>
        /// <param name="htmlAttributes">Html attributes</param>
        /// <returns>TextBox</returns>
        public static MvcHtmlString TextBox(this HtmlHelper helper, string name, int maxLength, WidthStyle widthStyle, object htmlAttributes)
        {
            var attributes = htmlAttributes == null ? new RouteValueDictionary() : new RouteValueDictionary(htmlAttributes);

            return helper.TextBox(name, null, maxLength, widthStyle, attributes);
        }

        /// <summary>
        /// TextBox
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <param name="name">Name</param>
        /// <param name="maxLength">Maxlength</param>
        /// <param name="value">Value</param>
        /// <param name="widthStyle">Width style</param>
        /// <returns>TextBox</returns>
        public static MvcHtmlString TextBox(this HtmlHelper helper, string name, object value, int maxLength, WidthStyle widthStyle)
        {
            return helper.TextBox(name, value, maxLength, widthStyle, null);
        }

        /// <summary>
        /// TextBox
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <param name="name">Name</param>
        /// <param name="maxLength">Maxlength</param>
        /// <param name="value">Value</param>
        /// <param name="widthStyle">Width style</param>
        /// <param name="htmlAttributes">Html attributes</param>
        /// <returns>TextBox</returns>
        public static MvcHtmlString TextBox(this HtmlHelper helper, string name, object value, int maxLength, WidthStyle widthStyle, object htmlAttributes)
        {
            var attributes = htmlAttributes == null ? new RouteValueDictionary() : new RouteValueDictionary(htmlAttributes);
            return helper.TextBox(name, value, maxLength, widthStyle, attributes);
        }

        /// <summary>
        /// TextBox
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <param name="name">Name</param>
        /// <param name="maxLength">Maxlength</param>
        /// <param name="value">Value</param>
        /// <param name="widthStyle">Width style</param>
        /// <param name="htmlAttributes">Html attributes</param>
        /// <returns>TextBox</returns>
        public static MvcHtmlString TextBox(this HtmlHelper helper, string name, object value, int maxLength, WidthStyle widthStyle, IDictionary<string, object> htmlAttributes)
        {
            var builder = new TagBuilder("input");

            builder.MergeAttributes(htmlAttributes);

            builder.MergeAttribute("type", "text", true);
            builder.MergeAttribute("name", name, true);
            builder.MergeAttribute("maxlength", maxLength.ToString(), true);
            builder.MergeAttribute("value", Convert.ToString(value, CultureInfo.CurrentCulture), true);
            builder.MergeAttribute("class", GetClass(widthStyle));

            if (htmlAttributes != null && htmlAttributes.ContainsKey("class"))
            {
                var @class = htmlAttributes.Where(htmlAttribute => htmlAttribute.Key.Equals("class"))
                    .Aggregate(GetClass(widthStyle), (current, htmlAttribute) => current + (" " + htmlAttribute.Value));

                builder.MergeAttribute("class", @class, true);
            }
            
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        /// Get class
        /// </summary>
        /// <param name="widthStyle">Width style</param>
        /// <returns>Class</returns>
        private static string GetClass(WidthStyle widthStyle)
        {
            var @class = "text-input";

            if(widthStyle.Equals(WidthStyle.Xsmall))
            {
                @class += " xsmall-input";
            }
            else if(widthStyle.Equals(WidthStyle.Small))
            {
                @class += " small-input";
            }
            else if (widthStyle.Equals(WidthStyle.Medium))
            {
                @class += " medium-input";
            }
            else if (widthStyle.Equals(WidthStyle.Large))
            {
                @class += " large-input";
            }

            return @class;
        }
    }
}
