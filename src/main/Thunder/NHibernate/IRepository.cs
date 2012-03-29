using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;

namespace Thunder.NHibernate
{
    /// <summary>
    /// Repository interface
    /// </summary>
    /// <typeparam name="T">Object type</typeparam>
    /// <typeparam name="TKey">Key type</typeparam>
    public interface IRepository<T, in TKey>
    {
        /// <summary>
        /// Get hibernate session
        /// </summary>
        ISession Session { get; }

        /// <summary>
        /// Add object in repository
        /// </summary>
        /// <param name="object">Object</param>
        /// <returns>Created object</returns>
        T Add(T @object);

        /// <summary>
        /// Update object in repository
        /// </summary>
        /// <param name="object">Object</param>
        /// <returns>Updated object</returns>
        T Update(T @object);

        /// <summary>
        /// Remove object of repository
        /// </summary>
        /// <param name="object">Object</param>
        void Remove(T @object);

        /// <summary>
        /// Remove entity repository
        /// </summary>
        /// <param name="id">Id</param>
        void Remove(TKey id);

        /// <summary>
        /// Find object by id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Object</returns>
        T FindById(TKey id);

        /// <summary>
        /// Find all entities
        /// </summary>
        /// <returns>Object list</returns>
        IList<T> FindAll();

        /// <summary>
        /// Find all entities with order
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>Object list sorted</returns>
        IList<T> FindAll(Order order);
    }
}