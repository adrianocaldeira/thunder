using System;
using System.Threading;
using System.Threading.Tasks;
using NHibernate.Event;
using NHibernate.Persister.Entity;
using Thunder.Data;

namespace Thunder.NHibernate.Pattern
{
    /// <summary>
    ///     Listener que preenche <c>Created</c> e <c>Updated</c> nas entidades que implementam
    ///     <see cref="ICreatedAndUpdatedProperty" />, sempre com o horário local do servidor:
    ///     na inserção, sobrescreve <c>Created</c> e <c>Updated</c> (mesmo que o objeto chegue
    ///     com valores preenchidos); na atualização, altera apenas <c>Updated</c>.
    /// </summary>
    [Serializable]
    public class CreatedAndUpdatedPropertyEventListener : IPreUpdateEventListener, IPreInsertEventListener
    {
        private static void Set(IEntityPersister persister, object[] state, string propertyName, object value)
        {
            var index = Array.IndexOf(persister.PropertyNames, propertyName);

            if (index == -1)
                return;

            state[index] = value;
        }

        private static void ApplyInsert(PreInsertEvent @event)
        {
            if (!(@event.Entity is ICreatedAndUpdatedProperty entity))
                return;

            var time = DateTime.Now; // horário local, sempre do servidor

            Set(@event.Persister, @event.State, "Created", time);
            Set(@event.Persister, @event.State, "Updated", time);

            entity.Created = time;
            entity.Updated = time;
        }

        private static void ApplyUpdate(PreUpdateEvent @event)
        {
            if (!(@event.Entity is ICreatedAndUpdatedProperty entity))
                return;

            var time = DateTime.Now;

            Set(@event.Persister, @event.State, "Updated", time);

            entity.Updated = time; // Created não é tocado na atualização
        }

        #region Implementation of IPreInsertEventListener

        /// <summary>
        ///     Sobrescreve <c>Created</c> e <c>Updated</c> com o horário local do servidor antes da inserção.
        /// </summary>
        /// <param name="event">Evento de pré-inserção.</param>
        /// <returns><c>false</c> — a operação nunca é vetada.</returns>
        public bool OnPreInsert(PreInsertEvent @event)
        {
            ApplyInsert(@event);
            return false;
        }

        /// <summary>
        ///     Sobrescreve <c>Created</c> e <c>Updated</c> com o horário local do servidor antes da inserção.
        /// </summary>
        /// <param name="event">Evento de pré-inserção.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns><c>false</c> — a operação nunca é vetada.</returns>
        public Task<bool> OnPreInsertAsync(PreInsertEvent @event, CancellationToken cancellationToken)
        {
            ApplyInsert(@event);
            return Task.FromResult(false);
        }

        #endregion

        #region Implementation of IPreUpdateEventListener

        /// <summary>
        ///     Atualiza apenas <c>Updated</c> com o horário local do servidor antes da atualização.
        /// </summary>
        /// <param name="event">Evento de pré-atualização.</param>
        /// <returns><c>false</c> — a operação nunca é vetada.</returns>
        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            ApplyUpdate(@event);
            return false;
        }

        /// <summary>
        ///     Atualiza apenas <c>Updated</c> com o horário local do servidor antes da atualização.
        /// </summary>
        /// <param name="event">Evento de pré-atualização.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns><c>false</c> — a operação nunca é vetada.</returns>
        public Task<bool> OnPreUpdateAsync(PreUpdateEvent @event, CancellationToken cancellationToken)
        {
            ApplyUpdate(@event);
            return Task.FromResult(false);
        }

        #endregion
    }
}
