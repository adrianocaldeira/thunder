using System.Collections.Generic;
using System.Linq;
using NHibernate;
using Thunder.Collections;

namespace Thunder.Data
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

        /// <summary>
        /// Paging data
        /// </summary>
        /// <typeparam name="T">{T}</typeparam>
        /// <param name="source">Source</param>
        /// <param name="currentPage">Current page</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Source paging</returns>
        public static Paging<T> Paging<T>(this IEnumerable<T> source, int currentPage, int pageSize)
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
