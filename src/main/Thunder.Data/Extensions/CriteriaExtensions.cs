using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using Thunder.Collections;
using Thunder.Collections.Extensions;
using Thunder.Model;

namespace Thunder.Data.Extensions
{
    /// <summary>
    /// Criteria extensions
    /// </summary>
    public static class CriteriaExtensions
    {
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

        /// <summary>
        /// Add orders with list
        /// </summary>
        /// <param name="source"><see cref="ICriteria"/></param>
        /// <param name="orders"><see cref="IList{T}"/></param>
        /// <returns><see cref="ICriteria"/></returns>
        public static ICriteria AddOrder(this ICriteria source, IList<Order> orders)
        {
            if (orders != null && orders.Any())
            {
                foreach (var order in orders)
                {
                    source.AddOrder(order);
                }
            }
            return source;
        }

        /// <summary>
        /// Add criterions with list
        /// </summary>
        /// <param name="source"><see cref="ICriteria"/></param>
        /// <param name="criterions"><see cref="IList{T}"/></param>
        /// <returns><see cref="ICriteria"/></returns>
        public static ICriteria Add(this ICriteria source, IList<ICriterion> criterions)
        {
            if (criterions != null && criterions.Any())
            {
                foreach (var criterion in criterions)
                {
                    source.Add(criterion);
                }
            }
            return source;
        }
    }
}
