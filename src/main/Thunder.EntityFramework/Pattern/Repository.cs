using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Thunder.Data;

namespace Thunder.EntityFramework.Pattern
{
    /// <summary>
    ///     Repositório base sobre Entity Framework 6.
    /// </summary>
    /// <remarks>
    ///     Aplica o padrão unit-of-work: <see cref="Add" />, <see cref="Update" /> e
    ///     <see cref="Delete(T)" />/<see cref="Delete(TKey)" /> apenas alteram o estado das entidades
    ///     rastreadas; a persistência só ocorre ao chamar <see cref="Save" /> ou
    ///     <see cref="SaveAsync" />. O <see cref="DbContext" /> é injetado e o seu ciclo de vida é
    ///     responsabilidade de quem o forneceu — o repositório não o descarta.
    /// </remarks>
    /// <typeparam name="T">Tipo da entidade persistida.</typeparam>
    /// <typeparam name="TKey">Tipo da chave da entidade.</typeparam>
    public abstract class Repository<T, TKey> : IRepository<T, TKey> where T : Persist<T, TKey>
    {
        /// <summary>
        ///     Inicializa uma nova instância de <see cref="Repository{T,TKey}" />.
        /// </summary>
        /// <param name="context">Contexto do Entity Framework a ser utilizado.</param>
        protected Repository(DbContext context)
        {
            Context = context;
        }

        /// <summary>
        ///     Obtém ou define o <see cref="DbContext" /> utilizado pelo repositório.
        /// </summary>
        public DbContext Context { get; set; }

        /// <summary>
        ///     Lista todas as entidades sem rastreamento (<c>AsNoTracking</c>).
        /// </summary>
        /// <returns><see cref="IQueryable{T}" /> com todas as entidades.</returns>
        public IQueryable<T> All()
        {
            return Context.Set<T>().AsNoTracking();
        }

        /// <summary>
        ///     Consulta entidades por predicado, sem rastreamento (<c>AsNoTracking</c>).
        /// </summary>
        /// <param name="predicate">Expressão de filtro.</param>
        /// <returns><see cref="IQueryable{T}" /> com as entidades que satisfazem o predicado.</returns>
        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return Context.Set<T>().AsNoTracking().Where(predicate);
        }

        /// <summary>
        ///     Obtém uma única entidade pela chave (rastreada).
        /// </summary>
        /// <param name="id">Chave da entidade.</param>
        /// <returns>A entidade encontrada, ou <c>null</c> se não existir.</returns>
        public T Single(TKey id)
        {
            return Context.Set<T>().Find(id);
        }

        /// <summary>
        ///     Obtém de forma assíncrona uma única entidade pela chave (rastreada).
        /// </summary>
        /// <param name="id">Chave da entidade.</param>
        /// <param name="cancellationToken">Token de cancelamento.</param>
        /// <returns>Tarefa com a entidade encontrada, ou <c>null</c> se não existir.</returns>
        public Task<T> SingleAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return Context.Set<T>().FindAsync(cancellationToken, id);
        }

        /// <summary>
        ///     Marca a entidade para inclusão. Não persiste até chamar <see cref="Save" />/<see cref="SaveAsync" />.
        /// </summary>
        /// <param name="entity">Entidade a incluir.</param>
        public void Add(T entity)
        {
            Context.Set<T>().Add(entity);
        }

        /// <summary>
        ///     Marca a entidade como modificada. Não persiste até chamar <see cref="Save" />/<see cref="SaveAsync" />.
        /// </summary>
        /// <param name="entity">Entidade a atualizar.</param>
        public void Update(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        ///     Marca a entidade para remoção. Não persiste até chamar <see cref="Save" />/<see cref="SaveAsync" />.
        /// </summary>
        /// <param name="entity">Entidade a remover.</param>
        public void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
        }

        /// <summary>
        ///     Localiza a entidade pela chave e a marca para remoção, se existir.
        ///     Não persiste até chamar <see cref="Save" />/<see cref="SaveAsync" />.
        /// </summary>
        /// <param name="id">Chave da entidade a remover.</param>
        public void Delete(TKey id)
        {
            var entity = Context.Set<T>().Find(id);
            if (entity != null) Context.Set<T>().Remove(entity);
        }

        /// <summary>
        ///     Persiste as alterações pendentes no contexto.
        /// </summary>
        public void Save()
        {
            Context.SaveChanges();
        }

        /// <summary>
        ///     Persiste de forma assíncrona as alterações pendentes no contexto.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelamento.</param>
        /// <returns>Tarefa com o número de registros afetados.</returns>
        public Task<int> SaveAsync(CancellationToken cancellationToken = default)
        {
            return Context.SaveChangesAsync(cancellationToken);
        }
    }
}
