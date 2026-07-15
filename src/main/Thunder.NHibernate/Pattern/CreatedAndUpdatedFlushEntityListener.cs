using System;
using System.Threading;
using System.Threading.Tasks;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Event.Default;
using Thunder.Data;

namespace Thunder.NHibernate.Pattern
{
    /// <summary>
    ///     Listener de <c>FlushEntity</c> que garante a persistência de <c>Updated</c> em entidades
    ///     <see cref="ICreatedAndUpdatedProperty" /> mapeadas com <c>dynamic-update="true"</c>.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Com <c>dynamic-update="true"</c>, o UPDATE contém apenas as colunas dirty — e os campos
    ///         dirty são calculados <b>antes</b> do evento <c>PreUpdate</c> disparar. Mutar o <c>state</c>
    ///         no <see cref="CreatedAndUpdatedPropertyEventListener" /> não inclui <c>Updated</c> no SQL
    ///         dinâmico; a coluna simplesmente não é gravada. Este listener corrige isso atuando no ponto
    ///         do pipeline em que o dirty-check acontece: define <c>Updated</c> na própria entidade
    ///         <b>antes</b> de delegar ao <see cref="DefaultFlushEntityEventListener" />, de modo que o
    ///         dirty-check padrão detecte a mudança e inclua a coluna no UPDATE dinâmico.
    ///     </para>
    ///     <para>
    ///         Divisão de responsabilidades com o <see cref="CreatedAndUpdatedPropertyEventListener" />:
    ///         o <c>PreInsert</c> continua cobrindo o insert (<c>Created</c> e <c>Updated</c>); este
    ///         listener cobre o update parcial de entidade attached (com snapshot carregado); e o
    ///         <c>PreUpdate</c> continua cobrindo o reattach sem snapshot (update completo) e gravando o
    ///         valor final no <c>state</c> vinculado ao SQL. Os três cooperam sem conflito.
    ///     </para>
    ///     <para>
    ///         Registro: adicione a entrada <c>ListenerType.FlushEntity</c> ao dicionário
    ///         <see cref="SessionManager.Listeners" /> contendo <b>apenas</b> esta classe — como
    ///         <c>Configuration.SetListeners</c> substitui o listener padrão do NHibernate, esta classe
    ///         herda de <see cref="DefaultFlushEntityEventListener" /> e delega ao <c>base</c> para
    ///         preservar o comportamento padrão do flush (não registre o listener padrão junto, ou o
    ///         flush executará duas vezes).
    ///     </para>
    /// </remarks>
    [Serializable]
    public class CreatedAndUpdatedFlushEntityListener : DefaultFlushEntityEventListener
    {
        /// <summary>
        ///     Carimba <c>Updated</c> quando a entidade attached está dirty e delega o flush ao
        ///     comportamento padrão, que detecta a mudança e inclui a coluna no UPDATE dinâmico.
        /// </summary>
        /// <param name="event">Evento de flush da entidade.</param>
        public override void OnFlushEntity(FlushEntityEvent @event)
        {
            CarimbaUpdatedSeDirty(@event);
            base.OnFlushEntity(@event);
        }

        /// <summary>
        ///     Carimba <c>Updated</c> quando a entidade attached está dirty e delega o flush ao
        ///     comportamento padrão, que detecta a mudança e inclui a coluna no UPDATE dinâmico.
        /// </summary>
        /// <param name="event">Evento de flush da entidade.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Tarefa que representa o flush da entidade.</returns>
        public override Task OnFlushEntityAsync(FlushEntityEvent @event, CancellationToken cancellationToken)
        {
            CarimbaUpdatedSeDirty(@event);
            return base.OnFlushEntityAsync(@event, cancellationToken);
        }

        private static void CarimbaUpdatedSeDirty(FlushEntityEvent @event)
        {
            if (!(@event.Entity is ICreatedAndUpdatedProperty entidade))
                return;

            var entry = @event.EntityEntry;

            // Só o caminho de UPDATE attached com snapshot: INSERT segue coberto pelo PreInsert;
            // reattach sem snapshot (LoadedState == null) gera update completo, coberto pelo PreUpdate.
            if (entry.Status != Status.Loaded || !entry.ExistsInDatabase || entry.LoadedState == null)
                return;

            var persister = entry.Persister;
            var estadoAtual = persister.GetPropertyValues(@event.Entity);
            var dirty = persister.FindDirty(estadoAtual, entry.LoadedState, @event.Entity, @event.Session);

            if (dirty == null || dirty.Length == 0)
                return; // nada mudou: não força UPDATE espúrio a cada flush

            entidade.Updated = DateTime.Now; // o dirty-check padrão (base) detecta e inclui a coluna
        }
    }
}
