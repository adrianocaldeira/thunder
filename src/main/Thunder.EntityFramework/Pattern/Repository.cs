using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Thunder.Data;

namespace Thunder.EntityFramework.Pattern
{
    /// <summary>
    ///     Repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract class Repository<T, TKey> : IDisposable,
        IRepository<T, TKey>
        where T : Persist<T, TKey>
    {
        /// <summary>
        ///     Initialize new instance of <see cref="Repository{T,TKey}" />
        /// </summary>
        /// <param name="context"></param>
        protected Repository(DbContext context)
        {
            Context = context;
        }

        /// <summary>
        ///     Get or se <see cref="DbContext" />
        /// </summary>
        public DbContext Context { get; set; }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     List all of <see cref="IQueryable{T}" />
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> All()
        {
            return Context.Set<T>();
        }

        /// <summary>
        ///     Find by predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>
        ///     <see cref="IQueryable{T}" />
        /// </returns>
        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            var query = Context.Set<T>().Where(predicate);
            return query;
        }

        /// <summary>
        ///     Get single object
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        public T Single(TKey id)
        {
            return Context.Set<T>().Find(id);
        }

        /// <summary>
        ///     Add entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public void Add(T entity)
        {
            Context.Set<T>().Add(entity);
            Context.SaveChanges();
        }

        /// <summary>
        ///     Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
            Context.SaveChanges();
        }

        /// <summary>
        ///     Update entity
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            Context.SaveChanges();
        }

        /// <summary>
        ///     Save
        /// </summary>
        public void Save()
        {
            Context.SaveChanges();
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (Context == null) return;

            Context.Dispose();
            Context = null;
        }
    }
}