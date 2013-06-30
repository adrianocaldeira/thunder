using System;
using System.Collections.Generic;

namespace Thunder.Extensions
{
    /// <summary>
    /// Integer extensions
    /// </summary>
    public static class IntegerExtensions
    {
        /// <summary>
        /// Times
        /// </summary>
        /// <param name="times">Times</param>
        /// <param name="action">Action</param>
        public static void Times(this int times, Action<int> action)
        {
            for (var i = 0; i < times; i++)
                action(i);
        }

        /// <summary>
        /// Times
        /// </summary>
        /// <param name="times">Times</param>
        /// <returns></returns>
        public static IList<int> Times(this int times)
        {
            var list = new List<int>();

            for (var i = 0; i < times; i++)
            {
                list.Add(i);
            }
            
            return list;
        }
    }
}