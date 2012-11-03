using System;
using System.Collections.Generic;
using System.Linq;

namespace Thunder.Collections.Extensions
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

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"><see cref="T:System.Collections.Generic.ICollection`1"/></param>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <param name="checkContain">Check if item contains in collection</param>
        public static void Add<T>(this ICollection<T> source, T item, bool checkContain)
        {
            if (checkContain)
            {
                if (!source.Contains(item))
                {
                    source.Add(item);
                }
            }
            else
                source.Add(item);
        }

        /// <summary>
        /// Adds an items to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"><see cref="T:System.Collections.Generic.ICollection`1"/></param>
        /// <param name="items">The objects to add to the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        /// <param name="checkContain">Check if item contains in collection</param>
        public static void Add<T>(this ICollection<T> source, IList<T> items, bool checkContain)
        {
            if (checkContain)
            {
                foreach (var item in items.Where(item => !source.Contains(item)))
                {
                    source.Add(item);
                }

            }
            else
            {
                foreach (var item in items)
                {
                    source.Add(item);
                }
            }
        }

        /// <summary>
        /// Adds an items to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"><see cref="T:System.Collections.Generic.ICollection`1"/></param>
        /// <param name="items">The objects to add to the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        public static void Add<T>(this ICollection<T> source, IList<T> items)
        {
            source.Add(items, true);
        }

        /// <summary>
        /// Remove an items from <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"><see cref="T:System.Collections.Generic.ICollection`1"/></param>
        /// <param name="items"><see cref="T:System.Collections.Generic.IList`1"/></param>
        public static void Remove<T>(this ICollection<T> source, IList<T> items)
        {
            foreach (var item in items)
            {
                source.Remove(item);
            }
        }
    }
}