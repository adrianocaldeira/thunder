using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Criterion;

namespace Thunder.Data.Pattern
{
    /// <summary>
    /// Repository
    /// </summary>
    /// <typeparam name="T">Type class</typeparam>
    /// <typeparam name="TKey">Type key</typeparam>
    public class Repository<T, TKey> : IRepository<T, TKey> where T : Persist<T, TKey>
    {
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
                entity.NotifyCreated();
                entity.NotifyUpdated();

                Session.Save(entity);

                transaction.Commit();

                return entity;
            }
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
                    entity.NotifyUpdated();
                    entity.NotifyCreated();

                    Session.Save(entity);
                }

                transaction.Commit();

                return entities.ToList();
            }
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
                entity.NotifyUpdated();

                Session.SaveOrUpdate(entity);

                transaction.Commit();

                return entity;
            }
        }

        /// <summary>
        /// Update property
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="name">Property Name</param>
        /// <param name="value">Property Value</param>
        /// <returns>Entity</returns>
        public T UpdateProperty<TProperty>(TKey id, string name, TProperty value)
        {
            return UpdateProperty(id, Property<TProperty>.Create(name, value));
        }

        /// <summary>
        /// Update property
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="id">Id</param>
        /// <param name="property">Property</param>
        /// <returns>Entity</returns>
        public T UpdateProperty<TProperty>(TKey id, Property<TProperty> property)
        {
            using (var transaction = Session.BeginTransaction())
            {
                var entity = Session.Get<T>(id);

                entity.GetType().GetProperty(property.Name).SetValue(entity, property.Value, null);
                entity.NotifyUpdated();

                Session.SaveOrUpdate(entity);

                transaction.Commit();

                return entity;
            }
        }

        /// <summary>
        /// Update properties
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="properties"><see cref="IList{T}"/></param>
        /// <returns>Entity</returns>
        public T UpdateProperties(TKey id, IList<Property<object>> properties)
        {
            using (var transaction = Session.BeginTransaction())
            {
                var entity = Session.Get<T>(id);

                foreach (var property in properties)
                {
                    entity.GetType().GetProperty(property.Name).SetValue(entity, property.Value, null);
                }

                entity.NotifyUpdated();

                Session.SaveOrUpdate(entity);

                transaction.Commit();

                return entity;
            }
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
        public T Find(TKey id)
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
        public IList<T> All()
        {
            using (var transaction = Session.BeginTransaction())
            {
                var list = Session.QueryOver<T>().List<T>();

                transaction.Commit();

                return list;
            }
        }

        /// <summary>
        /// All entities with expression
        /// </summary>
        /// <param name="expression">Expression</param>
        /// <returns><see cref="IList{T}"/></returns>
        public IList<T> All(Expression<Func<T, bool>> expression)
        {
            using (var transaction = Session.BeginTransaction())
            {
                var list = Session.QueryOver<T>().Where(expression).List();

                transaction.Commit();

                return list;
            }
        }

        /// <summary>
        /// All entities with criterion
        /// </summary>
        /// <param name="criterion"><see cref="ICriterion"/></param>
        /// <returns></returns>
        public IList<T> All(ICriterion criterion)
        {
            using (var transaction = Session.BeginTransaction())
            {
                var list = Session.QueryOver<T>().Where(criterion).List();

                transaction.Commit();

                return list;
            }
        }

        /// <summary>
        /// Find single entity from expression
        /// </summary>
        /// <param name="expression">Expression</param>
        /// <returns>Entity</returns>
        public T Single(Expression<Func<T, bool>> expression)
        {
            using (var transaction = Session.BeginTransaction())
            {
                var entity = Session.QueryOver<T>().Where(expression).SingleOrDefault<T>();

                transaction.Commit();

                return entity;
            }
        }

        /// <summary>
        /// Exist entity
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="expression">Expression</param>
        /// <returns>Exist</returns>
        public bool Exist(TKey id, Expression<Func<T, bool>> expression)
        {
            using (var transaction = Session.BeginTransaction())
            {
                var count = Session.QueryOver<T>()
                    .Select(Projections.CountDistinct("Id"))
                    .Where(Restrictions.Not(Restrictions.Eq("Id", id)))
                    .And(expression)
                    .SingleOrDefault<int>();

                transaction.Commit();

                return count > 0;
            }
        }

        /// <summary>
        /// Exist entity
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="criterions"><see cref="ICriterion"/></param>
        /// <returns>Exist</returns>
        public bool Exist(TKey id, params ICriterion[] criterions)
        {
            using (var transaction = Session.BeginTransaction())
            {
                var query = Session.QueryOver<T>()
                    .Select(Projections.CountDistinct("Id"))
                    .Where(Restrictions.Not(Restrictions.Eq("Id", id)));

                foreach (var criterion in criterions)
                {
                    query.And(criterion);
                }

                var count = query.SingleOrDefault<int>();

                transaction.Commit();

                return count > 0;
            }
        }

        #endregion
    }
}