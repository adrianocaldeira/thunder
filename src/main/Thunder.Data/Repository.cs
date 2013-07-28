using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Linq;

namespace Thunder.Data
{
    /// <summary>
    /// Repository
    /// </summary>
    /// <typeparam name="T">Type class</typeparam>
    /// <typeparam name="TKey">Type key</typeparam>
    public class Repository<T, TKey> : IRepository<T, TKey> where T : class
    {
        #region Implementation of IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<T> GetEnumerator()
        {
            return Session.Query<T>().GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of IQueryable

        /// <summary>
        /// Gets the expression tree that is associated with the instance of <see cref="T:System.Linq.IQueryable"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Linq.Expressions.Expression"/> that is associated with this instance of <see cref="T:System.Linq.IQueryable"/>.
        /// </returns>
        public Expression Expression
        {
            get { return Session.Query<T>().Expression; }
        }

        /// <summary>
        /// Gets the type of the element(s) that are returned when the expression tree associated with this instance of <see cref="T:System.Linq.IQueryable"/> is executed.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Type"/> that represents the type of the element(s) that are returned when the expression tree associated with this object is executed.
        /// </returns>
        public Type ElementType
        {
            get { return Session.Query<T>().ElementType; }
        }

        /// <summary>
        /// Gets the query provider that is associated with this data source.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Linq.IQueryProvider"/> that is associated with this data source.
        /// </returns>
        public IQueryProvider Provider
        {
            get { return Session.Query<T>().Provider; } 
        }

        #endregion

        #region Implementation of IRepository<T,in TKey>

        /// <summary>
        /// Get session
        /// </summary>
        public ISession Session
        {
            get { return SessionManager.CurrentSession; }
        }

        /// <summary>
        /// Create entity
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Entity</returns>
        public T Create(T entity)
        {
            using (var transaction = Session.BeginTransaction())
            {
                Session.Save(entity);
                transaction.Commit();
            }

            return entity;
        }

        /// <summary>
        /// Create entity 
        /// </summary>
        /// <param name="entities"><see cref="IList{T}"/></param>
        /// <returns><see cref="IList{T}"/></returns>
        public IList<T> Create(IList<T> entities)
        {
            using (var transaction = Session.BeginTransaction())
            {
                foreach (var entity in entities)
                {
                    Session.Save(entity);    
                }
                
                transaction.Commit();
            }

            return entities.ToList();
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Entity</returns>
        public T Update(T entity)
        {
            using (var transaction = Session.BeginTransaction())
            {
                Session.Update(entity);

                transaction.Commit();
            }

            return entity;
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="id">Id</param>
        public void Delete(TKey id)
        {
            using (var transaction = Session.BeginTransaction())
            {
                Session.Delete(Session.Get<T>(id));

                transaction.Commit();
            }
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public void Delete(T entity)
        {
            using (var transaction = Session.BeginTransaction())
            {
                Session.Delete(entity);

                transaction.Commit();
            }
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entities"><see cref="IEnumerable{T}"/></param>
        public void Delete(IEnumerable<T> entities)
        {
            using (var transaction = Session.BeginTransaction())
            {
                foreach (var entity in entities)
                {
                    Session.Delete(entity);    
                }

                transaction.Commit();
            }
        }

        /// <summary>
        /// Find entity by id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Entity</returns>
        public T FindById(TKey id)
        {
            using (var transaction = Session.BeginTransaction())
            {
                var entity = Session.Get<T>(id);
                transaction.Commit();
                return entity;
            }
        }

        /// <summary>
        /// All entities
        /// </summary>
        /// <returns><see cref="IQueryable{T}"/></returns>
        public IQueryable<T> All()
        {
            return Session.Query<T>();
        }

        /// <summary>
        /// Find single entity from expression
        /// </summary>
        /// <param name="expression"><see cref="Expression{TDelegate}"/></param>
        /// <returns>Entity</returns>
        public T Single(Expression<Func<T, bool>> expression)
        {
            return Find(expression).FirstOrDefault();
        }

        /// <summary>
        /// Find entities 
        /// </summary>
        /// <param name="expression"><see cref="Expression{TDelegate}"/></param>
        /// <returns><see cref="IQueryable{T}"/></returns>
        public IQueryable<T> Find(Expression<Func<T, bool>> expression)
        {
            return All().Where(expression).AsQueryable();
        }

        #endregion
    }
}