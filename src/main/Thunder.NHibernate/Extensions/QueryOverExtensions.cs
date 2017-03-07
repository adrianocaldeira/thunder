using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Criterion;
using Thunder.Collections;
using Thunder.Collections.Extensions;

namespace Thunder.NHibernate.Extensions
{
    /// <summary>
    /// Query over extensions
    /// </summary>
    public static class QueryOverExtensions
    {
        /// <summary>
        /// Paging extensions of <see cref="IQueryOver{TRoot}"/>
        /// </summary>
        /// <typeparam name="T">{T}</typeparam>
        /// <param name="queryOver"><see cref="IQueryOver{TRoot}"/></param>
        /// <param name="currentPage">Current page</param>
        /// <param name="pageSize">Page size</param>
        /// <returns><see cref="IPaging{T}"/></returns>
        public static IPaging<T> Paging<T>(this IQueryOver<T> queryOver, int currentPage, int pageSize)
        {
            return queryOver.Paging(currentPage, pageSize, null);
        }

        /// <summary>
        /// Paging extensions of <see cref="IQueryOver{TRoot}"/>
        /// </summary>
        /// <typeparam name="T">{T}</typeparam>
        /// <param name="queryOver"><see cref="IQueryOver{TRoot}"/></param>
        /// <param name="currentPage">Current page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="records">Records</param>
        /// <returns><see cref="IPaging{T}"/></returns>
        public static IPaging<T> Paging<T>(this IQueryOver<T> queryOver, int currentPage, int pageSize, long? records)
        {
            queryOver.Skip(currentPage * pageSize);
            queryOver.Take(pageSize);

            return records.HasValue
                       ? queryOver.List<T>().Paging(currentPage, pageSize, records.Value)
                       : queryOver.List<T>().Paging(currentPage, pageSize);
        }

        /// <summary>
        /// And
        /// </summary>
        /// <param name="source"><see cref="IQueryOver{TRoot}"/></param>
        /// <param name="criterions"><see cref="IList{T}"/></param>
        /// <typeparam name="T"></typeparam>
        /// <returns><see cref="IQueryOver{TRoot}"/></returns>
        public static IQueryOver<T, T> And<T>(this IQueryOver<T, T> source, IList<ICriterion> criterions)
        {
            if (criterions != null && criterions.Any())
            {
                foreach (var criterion in criterions)
                {
                    source.And(criterion);
                }
            }

            return source;
        }

        /// <summary>
        /// And
        /// </summary>
        /// <param name="source"><see cref="IQueryOver{TRoot}"/></param>
        /// <param name="expressions"><see cref="IList{T}"/></param>
        /// <typeparam name="T"></typeparam>
        /// <returns><see cref="IQueryOver{TRoot}"/></returns>
        public static IQueryOver<T, T> And<T>(this IQueryOver<T, T> source, IList<Expression<Func<T, bool>>> expressions)
        {
            if (expressions != null && expressions.Any())
            {
                foreach (var expression in expressions)
                {
                    source.And(expression);
                }
            }

            return source;
        }

        /// <summary>
        /// Order by
        /// </summary>
        /// <param name="source"><see cref="IQueryOver{TRoot}"/></param>
        /// <param name="orders"><see cref="IList{T}"/></param>
        /// <typeparam name="T"></typeparam>
        /// <returns><see cref="IQueryOver{TRoot}"/></returns>
        public static IQueryOver<T, T> OrderBy<T>(this IQueryOver<T, T> source, IList<Expression<Func<T, object>>> orders)
        {
            if (orders != null && orders.Any())
            {
                source.OrderBy(orders[0]);

                for (var i = 1; i < orders.Count; i++)
                {
                    source.ThenBy(orders[1]);
                }
            }

            return source;
        }
    }
}