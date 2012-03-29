using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;

namespace Thunder.NHibernate
{
    /// <summary>
    /// Generic Repository
    /// </summary>
    /// <typeparam name="T">Object</typeparam>
    /// <typeparam name="TKey">Id</typeparam>
    public class Repository<T, TKey> : IRepository<T, TKey>
    {
        /// <summary>
        /// Initialize new instance of <see cref="Repository{T,TKey}"/>.
        /// </summary>
        /// <param name="session">Hibernate session</param>
        public Repository(ISession session)
        {
            Session = session;
        }

        /// <summary>
        /// Get hibernate session instance
        /// </summary>
        public ISession Session { get; private set; }


        /// <summary>
        /// Add object in repository
        /// </summary>
        /// <param name="object">Object</param>
        /// <returns>Created object</returns>
        public T Add(T @object)
        {
            using (var transaction = Session.BeginTransaction())
            {
                Session.Save(@object);
                transaction.Commit();
            }

            return @object;
        }

        /// <summary>
        /// Update object in repository
        /// </summary>
        /// <param name="object">Object</param>
        /// <returns>Updated object</returns>
        public T Update(T @object)
        {
            using (var transaction = Session.BeginTransaction())
            {
                Session.SaveOrUpdate(@object);
                transaction.Commit();
            }

            return @object;
        }

        /// <summary>
        /// Remove object of repository
        /// </summary>
        /// <param name="object">Object</param>
        public void Remove(T @object)
        {
            using (var transaction = Session.BeginTransaction())
            {
                Session.Delete(@object);
                transaction.Commit();
            }
        }

        /// <summary>
        /// Remove entity repository
        /// </summary>
        /// <param name="id">Id</param>
        public void Remove(TKey id)
        {
            Remove(FindById(id));
        }

        /// <summary>
        /// Find object by id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Object</returns>
        public T FindById(TKey id)
        {
            T @object;

            using (var transaction = Session.BeginTransaction())
            {
                @object = Session.Get<T>(id);
                transaction.Commit();
            }

            return @object;
        }

        /// <summary>
        /// Find all entities
        /// </summary>
        /// <returns>Object list</returns>
        public IList<T> FindAll()
        {
            return FindAll(null);
        }

        /// <summary>
        /// Find all entities with order
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>Object list sorted</returns>
        public IList<T> FindAll(Order order)
        {
            IList<T> list;

            using (var transaction = Session.BeginTransaction())
            {
                var criteria = Session.CreateCriteria(typeof (T));

                if (order != null)
                    criteria.AddOrder(order);

                list = criteria.List<T>();

                transaction.Commit();
            }

            return list;
        }
    }
}