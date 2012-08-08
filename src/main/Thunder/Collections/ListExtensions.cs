using System;
using System.Collections.Generic;

namespace Thunder.Collections
{
    /// <summary>
    /// List extensions
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Get object of list
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="list">Liste</param>
        /// <param name="object">Object</param>
        /// <returns>Object</returns>
        public static T Get<T>(this IList<T> list, T @object)
        {
            return list.Index(@object) != -1 ? list[list.Index(@object)] : default(T);
        }

        /// <summary>
        /// Get object of list
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="list">List</param>
        /// <param name="index">Index</param>
        /// <returns>Object</returns>
        public static T Get<T>(this IList<T> list, int index)
        {
            try
            {
                return list[index];
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        ///<summary>
        /// Get index of list
        ///</summary>
        ///<param name="list">List</param>
        ///<param name="object">Object</param>
        ///<typeparam name="T">Type</typeparam>
        ///<returns>Index</returns>
        public static int Index<T>(this IList<T> list, T @object)
        {
            return list.IndexOf(@object);
        }
    }
}