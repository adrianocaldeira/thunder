using System;
using System.Linq.Expressions;

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
    }
}
