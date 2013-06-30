using NHibernate;
using Thunder.Collections;
using Thunder.Collections.Extensions;
using Thunder.Model;

namespace Thunder.Data.Extensions
{
    /// <summary>
    /// Page extensions
    /// </summary>
    public static class PagingExtensions
    {
        /// <summary>
        /// Paging extensions of <see cref="IQuery"/>
        /// </summary>
        /// <typeparam name="T">{T}</typeparam>
        /// <param name="query"><see cref="IQuery"/></param>
        /// <param name="currentPage">Current page</param>
        /// <param name="pageSize">Page size</param>
        /// <returns><see cref="IPaging{T}"/></returns>
        public static IPaging<T> Paging<T>(this IQuery query, int currentPage, int pageSize)
        {
            return query.Paging<T>(currentPage, pageSize, null);
        }

        /// <summary>
        /// Paging extensions of <see cref="IQuery"/> width <see cref="Filter"/>
        /// </summary>
        /// <param name="query"><see cref="IQuery"/></param>
        /// <param name="filter"><see cref="Filter"/></param>
        /// <typeparam name="T">{T}</typeparam>
        /// <returns><see cref="IPagingFilter{T}"/></returns>
        public static IPagingFilter<T> Paging<T>(this IQuery query, Filter filter)
        {
            return query.Paging<T>(filter, null);
        }

        /// <summary>
        /// Paging extensions of <see cref="IQuery"/>
        /// </summary>
        /// <typeparam name="T">{T}</typeparam>
        /// <param name="query"><see cref="IQuery"/></param>
        /// <param name="currentPage">Current page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="records">Records</param>
        /// <returns><see cref="IPaging{T}"/></returns>
        public static IPaging<T> Paging<T>(this IQuery query, int currentPage, int pageSize, long? records)
        {
            query.SetFirstResult(currentPage * pageSize);
            query.SetMaxResults(pageSize);
            
            return records.HasValue
                       ? query.List<T>().Paging(currentPage, pageSize, records.Value)
                       : query.List<T>().Paging(currentPage, pageSize);
        }

        /// <summary>
        /// Paging extensions of <see cref="IQuery"/> with <see cref="Filter"/>
        /// </summary>
        /// <param name="query"><see cref="IQuery"/></param>
        /// <param name="filter"><see cref="Filter"/></param>
        /// <param name="records">Recrods</param>
        /// <typeparam name="T">{T}</typeparam>
        /// <returns><see cref="IPagingFilter{T}"/></returns>
        public static IPagingFilter<T> Paging<T>(this IQuery query, Filter filter, long? records)
        {
            query.SetFirstResult(filter.CurrentPage * filter.PageSize);
            query.SetMaxResults(filter.PageSize);

            return records.HasValue
                       ? query.List<T>().Paging(filter, records.Value)
                       : query.List<T>().Paging(filter);
        }

        /// <summary>
        /// Paging extensions of <see cref="ICriteria"/>
        /// </summary>
        /// <typeparam name="T">{T}</typeparam>
        /// <param name="criteria"><see cref="ICriteria"/></param>
        /// <param name="currentPage">Current page</param>
        /// <param name="pageSize">Page size</param>
        /// <returns><see cref="IPaging{T}"/></returns>
        public static IPaging<T> Paging<T>(this ICriteria criteria, int currentPage, int pageSize)
        {
            return criteria.Paging<T>(currentPage, pageSize, null);
        }

        /// <summary>
        /// Paging extensions of <see cref="ICriteria"/> with <see cref="Filter"/>
        /// </summary>
        /// <param name="criteria"><see cref="ICriteria"/></param>
        /// <param name="filter"><see cref="Filter"/></param>
        /// <typeparam name="T">{T}</typeparam>
        /// <returns><see cref="IPagingFilter{T}"/></returns>
        public static IPagingFilter<T> Paging<T>(this ICriteria criteria, Filter filter)
        {
            return criteria.Paging<T>(filter, null);
        }

        /// <summary>
        /// Paging extensions of <see cref="ICriteria"/>
        /// </summary>
        /// <typeparam name="T">{T}</typeparam>
        /// <param name="criteria"><see cref="ICriteria"/></param>
        /// <param name="currentPage">Current page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="records">Records</param>
        /// <returns><see cref="IPaging{T}"/></returns>
        public static IPaging<T> Paging<T>(this ICriteria criteria, int currentPage, int pageSize, long? records)
        {
            criteria.SetFirstResult(currentPage * pageSize);
            criteria.SetMaxResults(pageSize);

            return records.HasValue
                       ? criteria.List<T>().Paging(currentPage, pageSize, records.Value)
                       : criteria.List<T>().Paging(currentPage, pageSize);
        }

        /// <summary>
        /// Paging extensions of <see cref="ICriteria"/> with filter
        /// </summary>
        /// <param name="criteria"><see cref="ICriteria"/></param>
        /// <param name="filter"><see cref="Filter"/></param>
        /// <param name="records">Records</param>
        /// <typeparam name="T">{T}</typeparam>
        /// <returns><see cref="IPagingFilter{T}"/></returns>
        public static IPagingFilter<T> Paging<T>(this ICriteria criteria, Filter filter, long? records)
        {
            criteria.SetFirstResult(filter.CurrentPage * filter.PageSize);
            criteria.SetMaxResults(filter.PageSize);

            return records.HasValue
                       ? criteria.List<T>().Paging(filter, records.Value)
                       : criteria.List<T>().Paging(filter);
        }
    }
}
