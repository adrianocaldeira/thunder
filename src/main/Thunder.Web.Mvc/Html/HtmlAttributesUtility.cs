using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Thunder.Web.Mvc.Html
{
    internal static class HtmlAttributesUtility
    {
        public static IDictionary<string, object> ObjectToHtmlAttributesDictionary(object htmlAttributes)
        {
            if (htmlAttributes == null)
            {
                return new Dictionary<string, object>();
            }

            return htmlAttributes as IDictionary<string, object> ?? 
                   HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
        }

        public static void AddMaxLengthAttribute<TModel, TProperty>(this IDictionary<string, object> attributes, 
            Expression<Func<TModel, TProperty>> expression, int? maximumLength = null)
        {
            var member = expression.Body as MemberExpression;
            var maximum = maximumLength;

            if (member != null)
            {
                var stringLengthAttribute = member.Member.GetCustomAttributes(typeof(StringLengthAttribute), false).FirstOrDefault() as StringLengthAttribute;

                if (stringLengthAttribute != null)
                {
                    maximum = stringLengthAttribute.MaximumLength;
                }
            }

            if (!attributes.ContainsKey("maxlength") && maximum.HasValue)
            {
                attributes.Add("maxlength", maximum);    
            }
        }

        public static void AddCssClass(this IDictionary<string, object> attributes, 
            string cssClass)
        {
            if (attributes.ContainsKey("class"))
            {
                attributes["class"] += " " + cssClass;
            }
            else
            {
                attributes.Add("class", cssClass);
            }
        }

        public static void MergeAttribute(this IDictionary<string, object> attributes, string key, object value)
        {
            attributes.MergeAttribute(key, value, false);
        }

        public static void MergeAttribute(this IDictionary<string, object> attributes, string key, object value, bool overrideValue)
        {
            if (attributes.ContainsKey(key))
            {
                if (overrideValue)
                {
                    attributes[key] = value;                    
                }
            }
            else
            {
                attributes.Add(key, value);
            }
        }
    }
}