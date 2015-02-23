using System;
using System.Linq;

namespace Thunder.Extensions
{
    /// <summary>
    /// Object extensions
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Cast object to type
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="source">Object</param>
        /// <returns>Object</returns>
        public static T Cast<T>(this object source)
        {
            if (source is string && string.IsNullOrEmpty(source.ToString()))
            {
                return default(T);
            }

            try
            {
                var type = typeof(T);
                var nullableType = Nullable.GetUnderlyingType(type);

                if (nullableType != null)
                {
                    return (T)Convert.ChangeType(source, nullableType);
                }

                return (T)Convert.ChangeType(source, type);
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// Call Trim function all string properties of object
        /// </summary>
        /// <param name="source"></param>
        public static void Trim<T>(this T source) where T : class 
        {
            var properties = source.GetType().GetProperties().Where(p => p.PropertyType == typeof(string) && p.CanWrite);

            foreach (var property in properties)
            {
                var currentValue = (string)property.GetValue(source, null);
                property.SetValue(source, currentValue == null ? null : currentValue.Trim(), null);
            }
        }
    }
}
