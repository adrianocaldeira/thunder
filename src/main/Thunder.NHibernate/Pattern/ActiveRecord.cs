using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using Thunder.Collections;
using Thunder.Collections.Extensions;
using Thunder.Data;
using Thunder.NHibernate.Extensions;
using Property = Thunder.Data.Property;

namespace Thunder.NHibernate.Pattern
{
    /// <summary>
    /// Active record
    /// </summary>
    /// <typeparam name="T">Class type</typeparam>
    /// <typeparam name="TKey">Key type</typeparam>
    [Obsolete("Use Thunder.NHibernate.Pattern.Repository<T, TKey>. Será removida na 3.0.")]
    public class ActiveRecord<T, TKey> : Persist<T, TKey> where T : Persist<T, TKey>
    {
        /// <summary>
        /// Merge list type of <see cref="IObjectState"/>
        /// </summary>
        /// <param name="current">Current list</param>
        /// <param name="list">List</param>
        /// <param name="propertiesForUpdate">Property</param>
        /// <typeparam name="TList"></typeparam>
        public virtual void Merge<TList>(IList<TList> current, IList<TList> list, params Expression<Func<TList, object>>[] propertiesForUpdate)
        {
            foreach (var item in list)
            {
                var state = ((IObjectState)item).State;

                if (state.Equals(ObjectState.Added))
                {
                    current.Add(item);
                }
                else if (state.Equals(ObjectState.Deleted))
                {
                    current.Remove(item);
                }
                else if (state.Equals(ObjectState.Modified))
                {
                    var currentItem = current.Get(item);

                    foreach (var property in propertiesForUpdate)
                    {
                        var propertyName = Utility.GetPropertyName(property);
                        var propertyInfoItem = item.GetType().GetProperty(propertyName);
                        var propertyInfoCurrentItem = currentItem.GetType().GetProperty(propertyName);

                        propertyInfoCurrentItem.SetValue(currentItem, Convert.ChangeType(propertyInfoItem.GetValue(item, null),
                            propertyInfoCurrentItem.PropertyType), null);
                    }
                }
            }
        }

        /// <summary>
        /// Get current session
        /// </summary>
        public static ISession Session
        {
            get { return SessionManager.SessionFactory.GetCurrentSession(); }
        }

        /// <summary>
        /// Create entity
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Entity</returns>
        public static T Create(T entity) => new Repository<T, TKey>().Create(entity);

        /// <summary>
        /// Create entity
        /// </summary>
        /// <param name="entities"><see cref="IList{T}"/></param>
        /// <returns><see cref="IList{T}"/></returns>
        public static IList<T> Create(IList<T> entities) => new Repository<T, TKey>().Create(entities);

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Entity</returns>
        public static T Update(T entity) => new Repository<T, TKey>().Update(entity);

        /// <summary>
        /// Update property
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="name">Property Name</param>
        /// <param name="value">Property Value</param>
        /// <returns>Entity</returns>
        public static T UpdateProperty<TProperty>(TKey id, string name, TProperty value) =>
            new Repository<T, TKey>().UpdateProperty(id, name, value);

        /// <summary>
        /// Update property
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="id">Id</param>
        /// <param name="property">Property</param>
        /// <returns>Entity</returns>
        public static T UpdateProperty<TProperty>(TKey id, Property<TProperty> property) =>
            new Repository<T, TKey>().UpdateProperty(id, property);

        /// <summary>
        /// Update properties
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="properties"><see cref="IList{T}"/></param>
        /// <returns>Entity</returns>
        public static T UpdateProperties(TKey id, IList<Property<object>> properties) =>
            new Repository<T, TKey>().UpdateProperties(id, properties);

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="id">Id</param>
        public static void Delete(TKey id) => new Repository<T, TKey>().Delete(id);

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public static void Delete(T entity) => new Repository<T, TKey>().Delete(entity);

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entities"><see cref="IEnumerable{T}"/></param>
        public static void Delete(IEnumerable<T> entities) => new Repository<T, TKey>().Delete(entities);

        /// <summary>
        /// Find entity by id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Entity</returns>
        public static T Find(TKey id) => new Repository<T, TKey>().Find(id);

        /// <summary>
        /// All entities
        /// </summary>
        /// <returns><see cref="IQueryable{T}"/></returns>
        public static IList<T> All() => new Repository<T, TKey>().All();

        /// <summary>
        /// All entities with expression
        /// </summary>
        /// <param name="expression">Expression</param>
        /// <returns><see cref="IList{T}"/></returns>
        public static IList<T> All(Expression<Func<T, bool>> expression) => new Repository<T, TKey>().All(expression);

        /// <summary>
        /// All entities with criterion
        /// </summary>
        /// <param name="criterion"><see cref="ICriterion"/></param>
        /// <returns></returns>
        public static IList<T> All(ICriterion criterion) => new Repository<T, TKey>().All(criterion);

        /// <summary>
        /// Find single entity from expression
        /// </summary>
        /// <param name="expression">Expression</param>
        /// <returns>Entity</returns>
        public static T Single(Expression<Func<T, bool>> expression) => new Repository<T, TKey>().Single(expression);

        /// <summary>
        /// Exist entity
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="expression">Expression</param>
        /// <returns>Exist</returns>
        public static bool Exist(TKey id, Expression<Func<T, bool>> expression) =>
            new Repository<T, TKey>().Exist(id, expression);

        /// <summary>
        /// Exist entity
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="criterions"><see cref="ICriterion"/></param>
        /// <returns>Exist</returns>
        public static bool Exist(TKey id, params ICriterion[] criterions) =>
            new Repository<T, TKey>().Exist(id, criterions);

        /// <summary>
        /// Page entity
        /// </summary>
        /// <param name="currentPage">Current Page</param>
        /// <param name="pageSize">Page Size</param>
        /// <returns><see cref="IPaging{T}"/></returns>
        public static IPaging<T> Page(int currentPage, int pageSize) =>
            new Repository<T, TKey>().Page(currentPage, pageSize);

        /// <summary>
        /// Page entity
        /// </summary>
        /// <param name="currentPage">Current Page</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="order"><see cref="Order"/></param>
        /// <returns><see cref="IList{T}"/></returns>
        public static IPaging<T> Page(int currentPage, int pageSize, Order order) =>
            new Repository<T, TKey>().Page(currentPage, pageSize, order);

        /// <summary>
        /// Page entity
        /// </summary>
        /// <param name="currentPage">Current Page</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="orders"><see cref="IList{T}"/></param>
        /// <returns><see cref="IPaging{T}"/></returns>
        public static IPaging<T> Page(int currentPage, int pageSize, IList<Order> orders) =>
            new Repository<T, TKey>().Page(currentPage, pageSize, orders);

        /// <summary>
        /// Page entity
        /// </summary>
        /// <param name="currentPage">Current Page</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="criterion">Criterion Expression</param>
        /// <returns><see cref="IPaging{T}"/></returns>
        public static IPaging<T> Page(int currentPage, int pageSize, Expression<Func<T, bool>> criterion) =>
            new Repository<T, TKey>().Page(currentPage, pageSize, criterion);

        /// <summary>
        /// Page entity
        /// </summary>
        /// <param name="currentPage">Current Page</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="criterion">Criterion Expression</param>
        /// <param name="order"><see cref="Order"/></param>
        /// <returns><see cref="IPaging{T}"/></returns>
        public static IPaging<T> Page(int currentPage, int pageSize, Expression<Func<T, bool>> criterion, Order order) =>
            new Repository<T, TKey>().Page(currentPage, pageSize, criterion, order);

        /// <summary>
        /// Page entity
        /// </summary>
        /// <param name="currentPage">Current Page</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="criterion">Criterion Expression</param>
        /// <param name="orders"><see cref="IList{T}"/></param>
        /// <returns><see cref="IPaging{T}"/></returns>
        public static IPaging<T> Page(int currentPage, int pageSize, Expression<Func<T, bool>> criterion, IList<Order> orders) =>
            new Repository<T, TKey>().Page(currentPage, pageSize, criterion, orders);

        /// <summary>
        /// Page entity
        /// </summary>
        /// <param name="currentPage">Current Page</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="criterions">Criterions Expression</param>
        /// <returns><see cref="IPaging{T}"/></returns>
        public static IPaging<T> Page(int currentPage, int pageSize, IList<Expression<Func<T, bool>>> criterions) =>
            new Repository<T, TKey>().Page(currentPage, pageSize, criterions);

        /// <summary>
        /// Page entity
        /// </summary>
        /// <param name="currentPage">Current Page</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="criterions">Criterions Expression</param>
        /// <param name="order"><see cref="Order"/></param>
        /// <returns><see cref="IPaging{T}"/></returns>
        public static IPaging<T> Page(int currentPage, int pageSize, IList<Expression<Func<T, bool>>> criterions, Order order) =>
            new Repository<T, TKey>().Page(currentPage, pageSize, criterions, order);

        /// <summary>
        /// Page entity
        /// </summary>
        /// <param name="currentPage">Current Page</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="criterions">Criterions Expression</param>
        /// <param name="orders"><see cref="IList{T}"/></param>
        /// <returns><see cref="IPaging{T}"/></returns>
        public static IPaging<T> Page(int currentPage, int pageSize, IList<Expression<Func<T, bool>>> criterions, IList<Order> orders) =>
            new Repository<T, TKey>().Page(currentPage, pageSize, criterions, orders);

        /// <summary>
        /// Page entity
        /// </summary>
        /// <param name="currentPage">Current Page</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="criterion"><see cref="ICriterion"/></param>
        /// <returns><see cref="IPaging{T}"/></returns>
        public static IPaging<T> Page(int currentPage, int pageSize, ICriterion criterion) =>
            new Repository<T, TKey>().Page(currentPage, pageSize, criterion);

        /// <summary>
        /// Page entity
        /// </summary>
        /// <param name="currentPage">Current Page</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="criterion"><see cref="ICriterion"/></param>
        /// <param name="order"><see cref="Order"/></param>
        /// <returns><see cref="IPaging{T}"/></returns>
        public static IPaging<T> Page(int currentPage, int pageSize, ICriterion criterion, Order order) =>
            new Repository<T, TKey>().Page(currentPage, pageSize, criterion, order);

        /// <summary>
        /// Page entity
        /// </summary>
        /// <param name="currentPage">Current Page</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="criterion"><see cref="ICriterion"/></param>
        /// <param name="orders"><see cref="IList{T}"/></param>
        /// <returns><see cref="IPaging{T}"/></returns>
        public static IPaging<T> Page(int currentPage, int pageSize, ICriterion criterion, IList<Order> orders) =>
            new Repository<T, TKey>().Page(currentPage, pageSize, criterion, orders);

        /// <summary>
        /// Page entity
        /// </summary>
        /// <param name="currentPage">Current Page</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="criterions"><see cref="IList{T}"/></param>
        /// <returns><see cref="IPaging{T}"/></returns>
        public static IPaging<T> Page(int currentPage, int pageSize, IList<ICriterion> criterions) =>
            new Repository<T, TKey>().Page(currentPage, pageSize, criterions);

        /// <summary>
        /// Page entity
        /// </summary>
        /// <param name="currentPage">Current Page</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="criterions"><see cref="IList{T}"/></param>
        /// <param name="order"><see cref="Order"/></param>
        /// <returns><see cref="IPaging{T}"/></returns>
        public static IPaging<T> Page(int currentPage, int pageSize, IList<ICriterion> criterions, Order order) =>
            new Repository<T, TKey>().Page(currentPage, pageSize, criterions, order);

        /// <summary>
        /// Page entity
        /// </summary>
        /// <param name="currentPage">Current Page</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="criterions"><see cref="IList{T}"/></param>
        /// <param name="orders"><see cref="Order"/></param>
        /// <returns><see cref="IPaging{T}"/></returns>
        public static IPaging<T> Page(int currentPage, int pageSize, IList<ICriterion> criterions, IList<Order> orders) =>
            new Repository<T, TKey>().Page(currentPage, pageSize, criterions, orders);
    }
}
