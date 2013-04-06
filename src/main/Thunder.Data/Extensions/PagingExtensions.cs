using NHibernate;
using Thunder.Collections;
using Thunder.Collections.Extensions;

namespace Thunder.Data.Extensions
{
    /// <summary>
    /// Page extensions
    /// </summary>
    public static class PagingExtensions
    {
        /// <summary>
        /// Paging query results
        /// </summary>
        /// <typeparam name="T">{T}</typeparam>
        /// <param name="query">Nhibernate query</param>
        /// <param name="currentPage">Current page</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Data paging</returns>
        public static IPaging<T> Paging<T>(this IQuery query, int currentPage, int pageSize)
        {
            return query.Paging<T>(currentPage, pageSize, null);
        }

        /// <summary>
        /// Paging query results
        /// </summary>
        /// <typeparam name="T">{T}</typeparam>
        /// <param name="query">Nhibernate query</param>
        /// <param name="currentPage">Current page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="records">Total records</param>
        /// <returns>Data paging</returns>
        public static IPaging<T> Paging<T>(this IQuery query, int currentPage, int pageSize, long? records)
        {
            query.SetFirstResult(currentPage * pageSize);
            query.SetMaxResults(pageSize);
            
            return records.HasValue
                       ? query.List<T>().Paging(currentPage, pageSize, records.Value)
                       : query.List<T>().Paging(currentPage, pageSize);
        }

        /// <summary>
        /// Paging criteria results
        /// </summary>
        /// <typeparam name="T">{T}</typeparam>
        /// <param name="criteria">Nhibernate query</param>
        /// <param name="currentPage">Current page</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Data paging</returns>
        public static IPaging<T> Paging<T>(this ICriteria criteria, int currentPage, int pageSize)
        {
            return criteria.Paging<T>(currentPage, pageSize, null);
        }

        /// <summary>
        /// Paging criteria results
        /// </summary>
        /// <typeparam name="T">{T}</typeparam>
        /// <param name="criteria">Nhibernate criteria</param>
        /// <param name="currentPage">Current page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="records">Total records</param>
        /// <returns>Data paging</returns>
        public static IPaging<T> Paging<T>(this ICriteria criteria, int currentPage, int pageSize, long? records)
        {
            criteria.SetFirstResult(currentPage * pageSize);
            criteria.SetMaxResults(pageSize);

            return records.HasValue
                       ? criteria.List<T>().Paging(currentPage, pageSize, records.Value)
                       : criteria.List<T>().Paging(currentPage, pageSize);
        }
    }
}
