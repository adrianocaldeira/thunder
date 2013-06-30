using System.Collections.Generic;
using System.Linq;
using Thunder.Model;

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
        /// Page data with filter
        /// </summary>
        /// <typeparam name="T">{T}</typeparam>
        /// <param name="source">Source</param>
        /// <param name="filter"><see cref="Filter"/></param>
        /// <returns><see cref="PagingFilter{T}"/></returns>
        public static PagingFilter<T> Paging<T>(this IEnumerable<T> source, Filter filter)
        {
            return new PagingFilter<T>(source, filter);
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
        /// Pagd data with filter
        /// </summary>
        /// <param name="source"><see cref="IEnumerable{T}"/></param>
        /// <param name="filter"><see cref="Filter"/></param>
        /// <param name="records">Records</param>
        /// <typeparam name="T">{T}</typeparam>
        /// <returns><see cref="PagingFilter{T}"/></returns>
        public static PagingFilter<T> Paging<T>(this IEnumerable<T> source, Filter filter, long records)
        {
            return new PagingFilter<T>(source, filter, records);
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
        /// Paged data with filter
        /// </summary>
        /// <param name="source"><see cref="IQueryable{T}"/></param>
        /// <param name="filter"><see cref="Filter"/></param>
        /// <typeparam name="T">{T}</typeparam>
        /// <returns><see cref="PagingFilter{T}"/></returns>
        public static PagingFilter<T> Paging<T>(this IQueryable<T> source, Filter filter)
        {
            return new PagingFilter<T>(source, filter);
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

        /// <summary>
        /// Paged data with filter
        /// </summary>
        /// <param name="source"><see cref="IQueryable{T}"/></param>
        /// <param name="filter"><see cref="Filter"/></param>
        /// <param name="records">Records</param>
        /// <typeparam name="T">{T}</typeparam>
        /// <returns><see cref="PagingFilter{T}"/></returns>
        public static PagingFilter<T> Paging<T>(this IQueryable<T> source, Filter filter, long records)
        {
            return new PagingFilter<T>(source, filter, records);
        }
    }
}
