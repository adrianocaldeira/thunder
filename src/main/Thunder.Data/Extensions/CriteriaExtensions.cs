using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using Thunder.Collections;
using Thunder.Collections.Extensions;

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
