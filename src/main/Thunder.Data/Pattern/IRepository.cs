using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Criterion;

namespace Thunder.Data.Pattern
{
    /// <summary>
    /// Repository interface
    /// </summary>
    /// <typeparam name="T">Class type</typeparam>
    /// <typeparam name="TKey">Key type</typeparam>
    public interface IRepository<T, in TKey> where T : class
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
        /// Update property
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="name">Property Name</param>
        /// <param name="value">Property Value</param>
        /// <returns>Entity</returns>
        T UpdateProperty<TProperty>(TKey id, string name, TProperty value);
        /// <summary>
        /// Update property
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="id">Id</param>
        /// <param name="property">Property</param>
        /// <returns>Entity</returns>
        T UpdateProperty<TProperty>(TKey id, Property<TProperty> property);
        /// <summary>
        /// Update properties
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="properties"><see cref="IList{T}"/></param>
        /// <returns>Entity</returns>
        T UpdateProperties(TKey id, IList<Property<object>> properties);

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
        T Find(TKey id);

        /// <summary>
        /// All entities
        /// </summary>
        /// <returns><see cref="IQueryable{T}"/></returns>
        IList<T> All();

        /// <summary>
        /// All entities with expression
        /// </summary>
        /// <param name="expression">Expression</param>
        /// <returns><see cref="IList{T}"/></returns>
        IList<T> All(Expression<Func<T, bool>> expression);

        /// <summary>
        /// All entities with criterion
        /// </summary>
        /// <param name="criterions"><see cref="ICriterion"/></param>
        /// <returns></returns>
        IList<T> All(ICriterion criterions);

        /// <summary>
        /// Find single entity from expression
        /// </summary>
        /// <param name="expression"><see cref="Expression{TDelegate}"/></param>
        /// <returns>Entity</returns>
        T Single(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Exist entity
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="expression">Expression</param>
        /// <returns>Exist</returns>
        bool Exist(TKey id, Expression<Func<T, bool>> expression);

        /// <summary>
        /// Exist entity
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="criterion"><see cref="ICriterion"/></param>
        /// <returns>Exist</returns>
        bool Exist(TKey id, params ICriterion[] criterion);
    }
}