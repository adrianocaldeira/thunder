using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Thunder
{
    /// <summary>
    /// Display Name
    /// </summary>
    public static class DisplayName
    {
        /// <summary>
        /// Get display name 
        /// </summary>
        /// <param name="propertyExpression"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string Get<T>(Expression<Func<T, object>> propertyExpression)
        {
            var memberInfo = Information(propertyExpression.Body);

            if (memberInfo == null)
            {
                throw new ArgumentException(
                    "No property reference expression was found.",
                    "propertyExpression");
            }

            var attr = memberInfo.Attribute<DisplayAttribute>(false);

            return attr == null ? memberInfo.Name : attr.Name;
        }

        private static MemberInfo Information(Expression propertyExpression)
        {
            var memberExpression = propertyExpression as MemberExpression;

            if (memberExpression == null)
            {
                var unaryExpression = propertyExpression as UnaryExpression;

                if (unaryExpression != null && unaryExpression.NodeType == ExpressionType.Convert)
                    memberExpression = unaryExpression.Operand as MemberExpression;
            }

            if (memberExpression != null && memberExpression.Member.MemberType == MemberTypes.Property)
                return memberExpression.Member;

            return null;
        }

        private static T Attribute<T>(this MemberInfo member, bool isRequired) where T : Attribute
        {
            var attribute = member.GetCustomAttributes(typeof(T), false).SingleOrDefault();

            if (attribute == null && isRequired)
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The {0} attribute must be defined on member {1}",
                        typeof(T).Name,
                        member.Name));
            }

            return (T)attribute;
        }
    }
}
