using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;
using Thunder.Model;

namespace Thunder.NHibernate.Extensions
{
    /// <summary>
    /// Filter extensions
    /// </summary>
    public static class FilterExtensions
    {
        /// <summary>
        /// Cast orders of filter for query
        /// </summary>
        /// <param name="source"><see cref="IList{T}"/></param>
        /// <returns></returns>
        public static IList<Order> CastForQuery(this IList<FilterOrder> source)
        {
            return source.Select(filterOrder => new Order(filterOrder.Column, filterOrder.Asc)).ToList();
        }
    }
}