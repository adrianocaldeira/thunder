using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Thunder.Data
{
    /// <summary>
    /// Active Record Property
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ActiveRecordProperty<T> where T : class
    {
        /// <summary>
        /// Get or set property name
        /// </summary>
        public Expression<Func<T, string>> Name { get; set; }

        /// <summary>
        /// Get or set value
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Get property info
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns><see cref="PropertyInfo"/></returns>
        public static PropertyInfo GetPropertyInfo(Expression<Func<T, string>> entity)
        {
            return typeof (T).GetProperty(((MemberExpression) entity.Body).Member.Name);
        }

        /// <summary>
        /// Set property value
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="property">Property</param>
        public static void SetValue(T entity, ActiveRecordProperty<T> property)
        {
            var propertyInfo = GetPropertyInfo(property.Name);
            propertyInfo.SetValue(entity, Convert.ChangeType(property.Value, propertyInfo.PropertyType), null);
        }

        /// <summary>
        /// Set property value
        /// </summary>
        /// <param name="entity">Value</param>
        /// <param name="propertyName">Name</param>
        /// <param name="propertyValue">Value</param>
        public static void SetValue(T entity, string propertyName, object propertyValue)
        {
            var propertyInfo = typeof(T).GetProperty(propertyName);
            propertyInfo.SetValue(entity, Convert.ChangeType(propertyValue, propertyInfo.PropertyType), null);
        }

        /// <summary>
        /// Create active record property
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="value">Value</param>
        /// <returns><see cref="ActiveRecordProperty{T}"/></returns>
        public static ActiveRecordProperty<T> Create(Expression<Func<T, string>> name, object value)
        {
            return new ActiveRecordProperty<T> {Name = name, Value = value};
        }
    }
}