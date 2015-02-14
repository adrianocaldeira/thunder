using System;

namespace Thunder.Extensions
{
    /// <summary>
    /// Generic extensions
    /// </summary>
    public static class GenericExtensions
    {
        /// <summary>
        /// Check is value it between values
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="lower"></param>
        /// <param name="upper"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool Between<T>(this T actual, T lower, T upper) where T : IComparable<T>
        {
            return actual.CompareTo(lower) >= 0 && actual.CompareTo(upper) <= 0;
        }
    }
}
