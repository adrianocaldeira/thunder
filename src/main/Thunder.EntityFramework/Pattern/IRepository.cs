using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Thunder.EntityFramework.Pattern
{
    /// <summary>
    ///     Contrato do repositório sobre Entity Framework 6.
    /// </summary>
    /// <remarks>
    ///     As operações de escrita (<see cref="Add" />, <see cref="Update" />,
    ///     <see cref="Delete(T)" />, <see cref="Delete(TKey)" />) apenas alteram o estado das
    ///     entidades; a persistência ocorre em <see cref="Save" />/<see cref="SaveAsync" />
    ///     (unit-of-work).
    /// </remarks>
    /// <typeparam name="T">Tipo da entidade persistida.</typeparam>
    /// <typeparam name="TKey">Tipo da chave da entidade.</typeparam>
    public interface IRepository<T, in TKey> where T : class
    {
        /// <summary>
        ///     Lista todas as entidades sem rastreamento (<c>AsNoTracking</c>).
        /// </summary>
        /// <returns><see cref="IQueryable{T}" /> com todas as entidades.</returns>
        IQueryable<T> All();

        /// <summary>
        ///     Consulta entidades por predicado, sem rastreamento (<c>AsNoTracking</c>).
        /// </summary>
        /// <param name="predicate">Expressão de filtro.</param>
        /// <returns><see cref="IQueryable{T}" /> com as entidades que satisfazem o predicado.</returns>
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);

        /// <summary>
        ///     Obtém uma única entidade pela chave (rastreada).
        /// </summary>
        /// <param name="id">Chave da entidade.</param>
        /// <returns>A entidade encontrada, ou <c>null</c> se não existir.</returns>
        T Single(TKey id);

        /// <summary>
        ///     Obtém de forma assíncrona uma única entidade pela chave (rastreada).
        /// </summary>
        /// <param name="id">Chave da entidade.</param>
        /// <param name="cancellationToken">Token de cancelamento.</param>
        /// <returns>Tarefa com a entidade encontrada, ou <c>null</c> se não existir.</returns>
        Task<T> SingleAsync(TKey id, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Marca a entidade para inclusão. Não persiste até chamar <see cref="Save" />/<see cref="SaveAsync" />.
        /// </summary>
        /// <param name="entity">Entidade a incluir.</param>
        void Add(T entity);

        /// <summary>
        ///     Marca a entidade como modificada. Não persiste até chamar <see cref="Save" />/<see cref="SaveAsync" />.
        /// </summary>
        /// <param name="entity">Entidade a atualizar.</param>
        void Update(T entity);

        /// <summary>
        ///     Marca a entidade para remoção. Não persiste até chamar <see cref="Save" />/<see cref="SaveAsync" />.
        /// </summary>
        /// <param name="entity">Entidade a remover.</param>
        void Delete(T entity);

        /// <summary>
        ///     Localiza a entidade pela chave e a marca para remoção, se existir.
        ///     Não persiste até chamar <see cref="Save" />/<see cref="SaveAsync" />.
        /// </summary>
        /// <param name="id">Chave da entidade a remover.</param>
        void Delete(TKey id);

        /// <summary>
        ///     Persiste as alterações pendentes no contexto.
        /// </summary>
        void Save();

        /// <summary>
        ///     Persiste de forma assíncrona as alterações pendentes no contexto.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelamento.</param>
        /// <returns>Tarefa com o número de registros afetados.</returns>
        Task<int> SaveAsync(CancellationToken cancellationToken = default);
    }
}
