using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;

namespace Thunder.Data
{
    /// <summary>
    /// Repository interface
    /// </summary>
    /// <typeparam name="T">Class type</typeparam>
    /// <typeparam name="TKey">Key type</typeparam>
    public interface IRepository<T, in TKey> : IQueryable<T> where T : class
    {
        /// <summary>
        /// Get session
        /// </summary>
        ISession Session { get; }

        /// <summary>
        /// Create entity
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Entity</returns>
        T Create(T entity);

        /// <summary>
        /// Create entity 
        /// </summary>
        /// <param name="entities"><see cref="IList{T}"/></param>
        /// <returns><see cref="IList{T}"/></returns>
        IList<T> Create(IList<T> entities);

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Entity</returns>
        T Update(T entity);

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="id">Id</param>
        void Delete(TKey id);

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Delete(T entity);

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entities"><see cref="IEnumerable{T}"/></param>
        void Delete(IEnumerable<T> entities);

        /// <summary>
        /// Find entity by id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Entity</returns>
        T FindById(TKey id);

        /// <summary>
        /// All entities
        /// </summary>
        /// <returns><see cref="IQueryable{T}"/></returns>
        IQueryable<T> All();

        /// <summary>
        /// Find single entity from expression
        /// </summary>
        /// <param name="expression"><see cref="Expression{TDelegate}"/></param>
        /// <returns>Entity</returns>
        T Single(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Find entities 
        /// </summary>
        /// <param name="expression"><see cref="Expression{TDelegate}"/></param>
        /// <returns><see cref="IQueryable{T}"/></returns>
        IQueryable<T> Find(Expression<Func<T, bool>> expression);
    }
}