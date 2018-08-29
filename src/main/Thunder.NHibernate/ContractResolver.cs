using System;
using Newtonsoft.Json.Serialization;
using NHibernate.Proxy;

namespace Thunder.NHibernate
{
    /// <summary>
    ///     NHibernate contract resolver
    /// </summary>
    public class ContractResolver : CamelCasePropertyNamesContractResolver
    {
        /// <summary>
        ///     Create contract
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        protected override JsonContract CreateContract(Type objectType)
        {
            return base.CreateContract(typeof(INHibernateProxy).IsAssignableFrom(objectType)
                ? objectType.BaseType
                : objectType);
        }
    }
}