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

namespace Thunder.Data
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
            ActiveRecordProperty<ActiveRecord<T, TKey>>.SetValue(this, "Updated", DateTime.Now);
        }

        /// <summary>
        /// Notify created object
        /// </summary>
        public virtual void NotifyCreated()
        {
            ActiveRecordProperty<ActiveRecord<T, TKey>>.SetValue(this, "Created", DateTime.Now);
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
        /// Find object by identification
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Object</returns>
        public static T FindById(TKey id)
        {
            using (var transaction = Session.BeginTransaction())
            {
                var @object = Session.Get<T>(id);
                transaction.Commit();
                return @object;
            }
        }

        /// <summary>
        /// CreateSchema object
        /// </summary>
        /// <param name="object">Object</param>
        /// <returns></returns>
        public static T Create(T @object)
        {
            using (var transaction = Session.BeginTransaction())
            {
                ActiveRecordProperty<T>.SetValue(@object, "Created", DateTime.Now);
                ActiveRecordProperty<T>.SetValue(@object, "Updated", DateTime.Now);
                
                Session.Save(@object);
                
                transaction.Commit();
            }

            return @object;
        }

        /// <summary>
        /// Update object
        /// </summary>
        /// <param name="object">Object</param>
        /// <returns>Object</returns>
        public static T Update(T @object)
        {
            using (var transaction = Session.BeginTransaction())
            {
                ActiveRecordProperty<T>.SetValue(@object, "Updated", DateTime.Now);

                Session.SaveOrUpdate(@object);
                
                transaction.Commit();
            }

            return @object;
        }

        /// <summary>
        /// Delete object
        /// </summary>
        /// <param name="object">Object</param>
        public static void Delete(T @object)
        {
            using (var transaction = Session.BeginTransaction())
            {
                Session.Delete(@object);
                transaction.Commit();
            }
        }

        /// <summary>
        /// Delete object from database
        /// </summary>
        /// <param name="id">Id</param>
        public static void Delete(TKey id)
        {
            Delete(FindById(id));
        }
        
        /// <summary>
        /// Find all with order
        /// </summary>
        /// <param name="orders">Orders</param>
        /// <returns></returns>
        public static IList<T> All(params Order[] orders)
        {
            using (var transaction = Session.BeginTransaction())
            {
                var criteria = Session.CreateCriteria(typeof(T));

                foreach(var order in orders)
                {
                    criteria.AddOrder(order);   
                }

                var list = criteria.List<T>();
                
                transaction.Commit();

                return list;
            }
        }

        /// <summary>
        /// Where
        /// </summary>
        /// <param name="criterions">Criterions</param>
        /// <returns><see cref="T:System.Collections.Generic.IList`1"/></returns>
        public static IList<T> Where(params ICriterion[] criterions)
        {
            using (var transaction = Session.BeginTransaction())
            {
                var criteria = Session.CreateCriteria(typeof(T));

                foreach (var criterion in criterions)
                {
                    criteria.Add(criterion);
                }
               
                var list = criteria.List<T>();

                transaction.Commit();

                return list;
            }
        }

        /// <summary>
        /// Exist 
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="criterions">Criterions</param>
        /// <returns>Exist</returns>
        public static bool Exist(TKey id, params ICriterion[] criterions)
        {
            using (var transaction = Session.BeginTransaction())
            {
                var criteria = Session.CreateCriteria(typeof(T))
                    .SetProjection(Projections.Id())
                    .Add(Restrictions.Not(Restrictions.Eq("Id", id)));

                foreach (var criterion in criterions)
                {
                    criteria.Add(criterion);
                }

                var list = criteria.List();

                transaction.Commit();

                return list.Count > 0;
            }
        }

        /// <summary>
        /// Page
        /// </summary>
        /// <param name="currentPage">Current page</param>
        /// <param name="pageSize">Page size</param>
        /// <returns><see cref="T:Thunder.Collections.IPaging`1"/></returns>
        public static IPaging<T> Page(int currentPage, int pageSize)
        {
            return Page(currentPage, pageSize, null, null);
        }

        /// <summary>
        /// Page
        /// </summary>
        /// <param name="currentPage">Current page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="orders">Orders</param>
        /// <returns><see cref="T:Thunder.Collections.IPaging`1"/></returns>
        public static IPaging<T> Page(int currentPage, int pageSize, IList<Order> orders)
        {
            return Page(currentPage, pageSize, null, orders);
        }

        /// <summary>
        /// Page
        /// </summary>
        /// <param name="currentPage">Current page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="criterions">Criterions</param>
        /// <returns><see cref="T:Thunder.Collections.IPaging`1"/></returns>
        public static IPaging<T> Page(int currentPage, int pageSize, IList<ICriterion> criterions)
        {
            return Page(currentPage, pageSize, criterions, null);
        }

        /// <summary>
        /// Page
        /// </summary>
        /// <param name="currentPage">Current page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="criterions">Criterions</param>
        /// <param name="orders">Orders</param>
        /// <returns><see cref="T:Thunder.Collections.IPaging`1"/></returns>
        public static IPaging<T> Page(int currentPage, int pageSize, IList<ICriterion> criterions, IList<Order> orders)
        {
            using (var transaction = Session.BeginTransaction())
            {
                var query = Session.CreateCriteria(typeof(T))
                    .SetResultTransformer(new DistinctRootEntityResultTransformer());

                var count = Session.CreateCriteria(typeof(T))
                    .SetProjection(Projections.CountDistinct("Id"));

                if (orders != null && orders.Any())
                {
                    foreach (var order in orders)
                    {
                        query.AddOrder(order);
                    }                    
                }

                if(criterions != null && criterions.Any())
                {
                    foreach (var criterion in criterions)
                    {
                        query.Add(criterion);
                        count.Add(criterion);
                    }
                }
                
                var list = query.Paging<T>(currentPage, pageSize, count.UniqueResult<int>());

                transaction.Commit();

                return list;
            }
        }

        /// <summary>
        /// Update properties
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="properties">Property</param>
        public static void UpdateProperties(TKey id, params ActiveRecordProperty<T>[] properties)
        {
            using (var transaction = Session.BeginTransaction())
            {
                var entity = Session.Get<T>(id);

                foreach (var property in properties)
                {
                    ActiveRecordProperty<T>.SetValue(entity, property);    
                }

                ActiveRecordProperty<T>.SetValue(entity, "Updated", DateTime.Now);
                
                Session.Update(entity);

                transaction.Commit();
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