using System;

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
    }
}
