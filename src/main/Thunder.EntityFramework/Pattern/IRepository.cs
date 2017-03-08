using System;
using System.Linq;
using System.Linq.Expressions;

namespace Thunder.EntityFramework.Pattern
{
    /// <summary>
    ///     Repository interface
    /// </summary>
    public interface IRepository<T, in TKey> where T : class
    {
        /// <summary>
        ///     List all of <see cref="IQueryable{T}" />
        /// </summary>
        /// <returns></returns>
        IQueryable<T> All();

        /// <summary>
        ///     Find by predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>
        ///     <see cref="IQueryable{T}" />
        /// </returns>
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);

        /// <summary>
        ///     Get single object
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        T Single(TKey id);

        /// <summary>
        ///     Add entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Add(T entity);

        /// <summary>
        ///     Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Delete(T entity);

        /// <summary>
        ///     Update entity
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);

        /// <summary>
        ///     Save
        /// </summary>
        void Save();
    }
}