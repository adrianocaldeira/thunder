using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using Thunder.Collections;
using Thunder.Collections.Extensions;
using Thunder.Data.Extensions;

namespace Thunder.Data.Pattern
{
    /// <summary>
    /// Active record
    /// </summary>
    /// <typeparam name="T">Class type</typeparam>
    /// <typeparam name="TKey">Key type</typeparam>
    public class ActiveRecord<T, TKey> where T : class
    {
        /// <summary>
        /// Get or set id
        /// </summary>
        public virtual TKey Id { get; set; }

        /// <summary>
        /// Get or set created date
        /// </summary>
        public virtual DateTime Created { get; set; }

        /// <summary>
        /// Get or set updated date
        /// </summary>
        public virtual DateTime Updated { get; set; }

        /// <summary>
        /// Is new object
        /// </summary>
        /// <returns></returns>
        public virtual bool IsNew()
        {
            var id = Convert.ChangeType(Id, TypeCode.Int64);
            return id == null || (Int64)id <= 0;
        }

        /// <summary>
        /// Notify updated object
        /// </summary>
        public virtual void NotifyUpdated()
        {
            Updated = DateTime.Now;
        }

        /// <summary>
        /// Notify created object
        /// </summary>
        public virtual void NotifyCreated()
        {
            Created = DateTime.Now;
        }

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
                    item.GetType().GetProperty("Updated").SetValue(item, 
                        Convert.ChangeType(DateTime.Now, TypeCode.DateTime), null);

                    item.GetType().GetProperty("Created").SetValue(item, 
                        Convert.ChangeType(DateTime.Now, TypeCode.DateTime), null);

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

                        currentItem.GetType().GetProperty("Updated").SetValue(currentItem, Convert.ChangeType(DateTime.Now, TypeCode.DateTime), null);
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
        public static T Create(T entity)
        {
            using (var transaction = Session.BeginTransaction())
            {
                Property.SetValue(entity, "Updated", DateTime.Now);
                Property.SetValue(entity, "Created", DateTime.Now);

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
        public static IList<T> Create(IList<T> entities)
        {
            using (var transaction = Session.BeginTransaction())
            {
                foreach (var entity in entities)
                {
                    Property.SetValue(entity, "Updated", DateTime.Now);
                    Property.SetValue(entity, "Created", DateTime.Now);

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
        public static T Update(T entity)
        {
            using (var transaction = Session.BeginTransaction())
            {
                Property.SetValue(entity, "Updated", DateTime.Now);

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
        public static T UpdateProperty<TProperty>(TKey id, string name, TProperty value)
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
        public static T UpdateProperty<TProperty>(TKey id, Property<TProperty> property)
        {
            using (var transaction = Session.BeginTransaction())
            {
                var entity = Session.Get<T>(id);

                Property.SetValue(entity, property);
                Property.SetValue(entity, "Updated", DateTime.Now);
                
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
        public static T UpdateProperties(TKey id, IList<Property<object>> properties)
        {
            using (var transaction = Session.BeginTransaction())
            {
                var entity = Session.Get<T>(id);

                foreach (var property in properties)
                {
                    Property.SetValue(entity, property);
                }
                
                Property.SetValue(entity, "Updated", DateTime.Now);

                Session.SaveOrUpdate(entity);

                transaction.Commit();

                return entity;
            }
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="id">Id</param>
        public static void Delete(TKey id)
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
        public static void Delete(T entity)
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
        public static void Delete(IEnumerable<T> entities)
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
        public static T Find(TKey id)
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
        public static IList<T> All()
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
        public static IList<T> All(Expression<Func<T, bool>> expression)
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
        public static IList<T> All(ICriterion criterion)
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
        public static T Single(Expression<Func<T, bool>> expression)
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
        public static bool Exist(TKey id, Expression<Func<T, bool>> expression)
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
        public static bool Exist(TKey id, params ICriterion[] criterions)
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

        /// <summary>
        /// Page entity
        /// </summary>
        /// <param name="currentPage">Current Page</param>
        /// <param name="pageSize">Page Size</param>
        /// <returns><see cref="IPaging{T}"/></returns>
        public static IPaging<T> Page(int currentPage, int pageSize)
        {
            return Page(currentPage, pageSize, Order.Asc("Id"));
        }

        /// <summary>
        /// Page entity
        /// </summary>
        /// <param name="currentPage">Current Page</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="order"><see cref="Order"/></param>
        /// <returns><see cref="IList{T}"/></returns>
        public static IPaging<T> Page(int currentPage, int pageSize, Order order)
        {
            return Page(currentPage, pageSize, new List<Order> { order });
        }

        /// <summary>
        /// Page entity
        /// </summary>
        /// <param name="currentPage">Current Page</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="orders"><see cref="IList{T}"/></param>
        /// <returns><see cref="IPaging{T}"/></returns>
        public static IPaging<T> Page(int currentPage, int pageSize, IList<Order> orders)
        {
            using (var transaction = Session.BeginTransaction())
            {
                var queryResult = Session.QueryOver<T>()
                    .TransformUsing(new DistinctRootEntityResultTransformer())
                    .UnderlyingCriteria.AddOrder(orders);

                var queryCount = Session.QueryOver<T>()
                    .Select(Projections.CountDistinct("Id"));

                var list = queryResult.Paging<T>(currentPage, pageSize, queryCount.SingleOrDefault<int>());

                transaction.Commit();

                return list;
            }
        }

        /// <summary>
        /// Page entity
        /// </summary>
        /// <param name="currentPage">Current Page</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="criterion">Criterion Expression</param>
        /// <returns><see cref="IPaging{T}"/></returns>
        public static IPaging<T> Page(int currentPage, int pageSize, Expression<Func<T, bool>> criterion)
        {
            return Page(currentPage, pageSize, criterion, Order.Asc("Id"));
        }

        /// <summary>
        /// Page entity
        /// </summary>
        /// <param name="currentPage">Current Page</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="criterion">Criterion Expression</param>
        /// <param name="order"><see cref="Order"/></param>
        /// <returns><see cref="IPaging{T}"/></returns>
        public static IPaging<T> Page(int currentPage, int pageSize, Expression<Func<T, bool>> criterion, Order order)
        {
            return Page(currentPage, pageSize, criterion, new List<Order> { order });
        }

        /// <summary>
        /// Page entity
        /// </summary>
        /// <param name="currentPage">Current Page</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="criterion">Criterion Expression</param>
        /// <param name="orders"><see cref="IList{T}"/></param>
        /// <returns><see cref="IPaging{T}"/></returns>
        public static IPaging<T> Page(int currentPage, int pageSize, Expression<Func<T, bool>> criterion, IList<Order> orders)
        {
            return Page(currentPage, pageSize, new List<Expression<Func<T, bool>>> { criterion }, orders);
        }

        /// <summary>
        /// Page entity
        /// </summary>
        /// <param name="currentPage">Current Page</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="criterions">Criterions Expression</param>
        /// <returns><see cref="IPaging{T}"/></returns>
        public static IPaging<T> Page(int currentPage, int pageSize, IList<Expression<Func<T, bool>>> criterions)
        {
            return Page(currentPage, pageSize, criterions, Order.Asc("Id"));
        }

        /// <summary>
        /// Page entity
        /// </summary>
        /// <param name="currentPage">Current Page</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="criterions">Criterions Expression</param>
        /// <param name="order"><see cref="Order"/></param>
        /// <returns><see cref="IPaging{T}"/></returns>
        public static IPaging<T> Page(int currentPage, int pageSize, IList<Expression<Func<T, bool>>> criterions, Order order)
        {
            return Page(currentPage, pageSize, criterions, new List<Order> { order });
        }

        /// <summary>
        /// Page entity
        /// </summary>
        /// <param name="currentPage">Current Page</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="criterions">Criterions Expression</param>
        /// <param name="orders"><see cref="IList{T}"/></param>
        /// <returns><see cref="IPaging{T}"/></returns>
        public static IPaging<T> Page(int currentPage, int pageSize, IList<Expression<Func<T, bool>>> criterions, IList<Order> orders)
        {
            using (var transaction = Session.BeginTransaction())
            {
                var queryResult = Session.QueryOver<T>()
                    .TransformUsing(new DistinctRootEntityResultTransformer())
                    .And(criterions)
                    .UnderlyingCriteria.AddOrder(orders);

                var queryCount = Session.QueryOver<T>()
                    .And(criterions)
                    .Select(Projections.CountDistinct("Id"));

                var list = queryResult.Paging<T>(currentPage, pageSize, queryCount.SingleOrDefault<int>());

                transaction.Commit();

                return list;
            }
        }

        /// <summary>
        /// Page entity
        /// </summary>
        /// <param name="currentPage">Current Page</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="criterion"><see cref="ICriterion"/></param>
        /// <returns><see cref="IPaging{T}"/></returns>
        public static IPaging<T> Page(int currentPage, int pageSize, ICriterion criterion)
        {
            return Page(currentPage, pageSize, criterion, Order.Asc("Id"));
        }

        /// <summary>
        /// Page entity
        /// </summary>
        /// <param name="currentPage">Current Page</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="criterion"><see cref="ICriterion"/></param>
        /// <param name="order"><see cref="Order"/></param>
        /// <returns><see cref="IPaging{T}"/></returns>
        public static IPaging<T> Page(int currentPage, int pageSize, ICriterion criterion, Order order)
        {
            return Page(currentPage, pageSize, criterion, new List<Order> { order });
        }

        /// <summary>
        /// Page entity
        /// </summary>
        /// <param name="currentPage">Current Page</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="criterion"><see cref="ICriterion"/></param>
        /// <param name="orders"><see cref="IList{T}"/></param>
        /// <returns><see cref="IPaging{T}"/></returns>
        public static IPaging<T> Page(int currentPage, int pageSize, ICriterion criterion, IList<Order> orders)
        {
            return Page(currentPage, pageSize, new List<ICriterion> { criterion }, orders);
        }

        /// <summary>
        /// Page entity
        /// </summary>
        /// <param name="currentPage">Current Page</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="criterions"><see cref="IList{T}"/></param>
        /// <returns><see cref="IPaging{T}"/></returns>
        public static IPaging<T> Page(int currentPage, int pageSize, IList<ICriterion> criterions)
        {
            return Page(currentPage, pageSize, criterions, Order.Asc("Id"));
        }

        /// <summary>
        /// Page entity
        /// </summary>
        /// <param name="currentPage">Current Page</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="criterions"><see cref="IList{T}"/></param>
        /// <param name="order"><see cref="Order"/></param>
        /// <returns><see cref="IPaging{T}"/></returns>
        public static IPaging<T> Page(int currentPage, int pageSize, IList<ICriterion> criterions, Order order)
        {
            return Page(currentPage, pageSize, criterions, new List<Order> { order });
        }

        /// <summary>
        /// Page entity
        /// </summary>
        /// <param name="currentPage">Current Page</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="criterions"><see cref="IList{T}"/></param>
        /// <param name="orders"><see cref="Order"/></param>
        /// <returns><see cref="IPaging{T}"/></returns>
        public static IPaging<T> Page(int currentPage, int pageSize, IList<ICriterion> criterions, IList<Order> orders)
        {
            using (var transaction = Session.BeginTransaction())
            {
                var queryResult = Session.QueryOver<T>()
                    .TransformUsing(new DistinctRootEntityResultTransformer())
                    .And(criterions)
                    .UnderlyingCriteria.AddOrder(orders);

                var queryCount = Session.QueryOver<T>()
                    .Select(Projections.CountDistinct("Id"))
                    .And(criterions);

                var list = queryResult.Paging<T>(currentPage, pageSize, queryCount.SingleOrDefault<int>());

                transaction.Commit();

                return list;
            }
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var compare = obj as T;

            if (compare == null)
            {
                return false;
            }

            return (GetHashCode() == compare.GetHashCode());
        }

        /// <summary>
        /// GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (29 * Id.GetHashCode());
            }
        }
    }
}