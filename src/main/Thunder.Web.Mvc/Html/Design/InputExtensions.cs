using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Thunder.Web.Mvc.Html.Design
{
    /// <summary>
    /// Text box extensions
    /// </summary>
    public static class InputExtensions
    {
        /// <summary>
        /// TextBox
        /// </summary>
        /// <typeparam name="TModel">Model</typeparam>
        /// <typeparam name="TProperty">Property</typeparam>
        /// <param name="htmlHelper">Helper</param>
        /// <param name="expression">Expression</param>
        /// <param name="style">Style</param>
        /// <returns><see cref="MvcHtmlString"/></returns>
        public static MvcHtmlString TextBoxFor<TModel, TProperty>(
            this ThunderHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            TextBoxStyle style)
        {
            return htmlHelper.TextBoxFor(expression, style, null);
        }

        /// <summary>
        /// TextBox
        /// </summary>
        /// <param name="htmlHelper">Helper</param>
        /// <param name="expression">Expression</param>
        /// <param name="style">Style</param>
        /// <param name="htmlAttributes"></param>
        /// <typeparam name="TModel">Model</typeparam>
        /// <typeparam name="TProperty">Property</typeparam>
        /// <returns><see cref="MvcHtmlString"/></returns>
        public static MvcHtmlString TextBoxFor<TModel, TProperty>(this ThunderHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,TextBoxStyle style, object htmlAttributes
        )
        {
            var member = expression.Body as MemberExpression;
            var attributes = (IDictionary<string, object>)new RouteValueDictionary(htmlAttributes);

            if (attributes.ContainsKey("class"))
            {
                attributes["class"] = String.Concat(attributes["class"], " ", GetClass(style));
            }
            else
            {
                attributes["class"] = GetClass(style);
            }

            if (member != null)
            {
                var stringLength = member.Member.GetCustomAttributes(typeof(StringLengthAttribute), false)
                        .FirstOrDefault() as StringLengthAttribute;

                if (stringLength != null)
                {
                    attributes.Add("maxlength", stringLength.MaximumLength);
                }
            }

            return htmlHelper.Html.TextBoxFor(expression, attributes);
        }

        /// <summary>
        /// Password
        /// </summary>
        /// <typeparam name="TModel">Model</typeparam>
        /// <typeparam name="TProperty">Property</typeparam>
        /// <param name="htmlHelper"><see cref="ThunderHelper{TModel}"/></param>
        /// <param name="expression"><see cref="Expression{TDelegate}"/></param>
        /// <param name="style"><see cref="TextBoxStyle"/></param>
        /// <returns></returns>
        public static MvcHtmlString PasswordFor<TModel, TProperty>(this ThunderHelper<TModel> htmlHelper,
                Expression<Func<TModel, TProperty>> expression, TextBoxStyle style)
        {
            return htmlHelper.PasswordFor(expression, style, null);
        }

        /// <summary>
        /// Password
        /// </summary>
        /// <param name="htmlHelper">Helper</param>
        /// <param name="expression">Expression</param>
        /// <param name="style">Style</param>
        /// <param name="htmlAttributes"></param>
        /// <typeparam name="TModel">Model</typeparam>
        /// <typeparam name="TProperty">Property</typeparam>
        /// <returns><see cref="MvcHtmlString"/></returns>
        public static MvcHtmlString PasswordFor<TModel, TProperty>(this ThunderHelper<TModel> htmlHelper, 
            Expression<Func<TModel, TProperty>> expression, TextBoxStyle style, object htmlAttributes)
        {
            var member = expression.Body as MemberExpression;
            var attributes = (IDictionary<string, object>)new RouteValueDictionary(htmlAttributes);

            if (attributes.ContainsKey("class"))
            {
                attributes["class"] = String.Concat(attributes["class"], " ", GetClass(style));
            }
            else
            {
                attributes["class"] = GetClass(style);
            }

            if (member != null)
            {
                var stringLength = member.Member.GetCustomAttributes(typeof(StringLengthAttribute), false)
                        .FirstOrDefault() as StringLengthAttribute;

                if (stringLength != null)
                {
                    attributes.Add("maxlength", stringLength.MaximumLength);
                }
            }

            return htmlHelper.Html.PasswordFor(expression, attributes);
        }

        /// <summary>
        /// Get class style
        /// </summary>
        /// <param name="textBoxStyle">Width style</param>
        /// <returns>Class</returns>
        private static string GetClass(TextBoxStyle textBoxStyle)
        {
            var @class = "text-input";

            if(textBoxStyle.Equals(TextBoxStyle.Xsmall))
            {
                @class += " xsmall-input";
            }
            else if(textBoxStyle.Equals(TextBoxStyle.Small))
            {
                @class += " small-input";
            }
            else if (textBoxStyle.Equals(TextBoxStyle.Medium))
            {
                @class += " medium-input";
            }
            else if (textBoxStyle.Equals(TextBoxStyle.Large))
            {
                @class += " large-input";
            }

            return @class;
        }
    }
}
