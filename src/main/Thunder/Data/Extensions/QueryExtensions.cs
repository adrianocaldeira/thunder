using NHibernate;
using Thunder.Collections;
using Thunder.Collections.Extensions;

namespace Thunder.Data.Extensions
{
    /// <summary>
    /// Query extensions
    /// </summary>
    public static class QueryExtensions
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
    }
}
