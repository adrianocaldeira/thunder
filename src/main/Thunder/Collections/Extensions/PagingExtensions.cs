using System.Collections.Generic;
using System.Linq;

namespace Thunder.Collections.Extensions
{
    /// <summary>
    /// Page extensions
    /// </summary>
    public static class PagingExtensions
    {
        /// <summary>
        /// Page data
        /// </summary>
        /// <typeparam name="T">{T}</typeparam>
        /// <param name="source"><see cref="IEnumerable{T}"/></param>
        /// <param name="currentPage">Current page</param>
        /// <param name="pageSize">Page size</param>
        /// <returns><see cref="Paging{T}(System.Collections.Generic.IEnumerable{T},int,int)"/></returns>
        public static Paging<T> Paging<T>(this IEnumerable<T> source, int currentPage, int pageSize)
        {
            return new Paging<T>(source, currentPage, pageSize);
        }

        /// <summary>
        /// Paging data
        /// </summary>
        /// <typeparam name="T">{T}</typeparam>
        /// <param name="source"><see cref="IEnumerable{T}"/></param>
        /// <param name="currentPage">Current page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="records">Records</param>
        /// <returns><see cref="Paging{T}(System.Collections.Generic.IEnumerable{T},int,int)"/></returns>
        public static Paging<T> Paging<T>(this IEnumerable<T> source, int currentPage, int pageSize, long records)
        {
            return new Paging<T>(source, currentPage, pageSize, records);
        }

        /// <summary>
        /// Paging data
        /// </summary>
        /// <typeparam name="T">{T}</typeparam>
        /// <param name="source">Source</param>
        /// <param name="currentPage">Current page</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Source paging</returns>
        public static Paging<T> Paging<T>(this IQueryable<T> source, int currentPage, int pageSize)
        {
            return new Paging<T>(source, currentPage, pageSize);
        }

        /// <summary>
        /// Paging data
        /// </summary>
        /// <typeparam name="T">{T}</typeparam>
        /// <param name="source">Source</param>
        /// <param name="currentPage">Current page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="records">Records</param>
        /// <returns>Source paging</returns>
        public static Paging<T> Paging<T>(this IQueryable<T> source, int currentPage, int pageSize, long records)
        {
            return new Paging<T>(source, currentPage, pageSize, records);
        }
    }
}
