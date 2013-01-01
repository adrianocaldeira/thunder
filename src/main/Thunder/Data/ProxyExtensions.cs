using NHibernate.Proxy;

namespace Thunder.Data
{
    /// <summary>
    /// Hibernate proxy extension
    /// </summary>
    public static class ProxyExtensions
    {
        ///<summary>
        /// Unproxy 
        ///</summary>
        ///<param name="entity">Entity</param>
        ///<typeparam name="T">Type</typeparam>
        ///<returns></returns>
        public static T Unproxy<T>(this T entity)
        {
            var proxy = entity as INHibernateProxy;

            if (proxy != null)
                return (T)proxy.HibernateLazyInitializer.GetImplementation();

            return entity;
        }
    }
}
