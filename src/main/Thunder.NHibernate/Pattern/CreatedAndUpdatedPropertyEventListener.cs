using System;
using System.Threading;
using System.Threading.Tasks;
using NHibernate.Event;
using NHibernate.Persister.Entity;
using Thunder.Data;

namespace Thunder.NHibernate.Pattern
{
    /// <summary>
    /// Created and updated property event listener
    /// </summary>
    [Serializable]
    public class CreatedAndUpdatedPropertyEventListener : IPreUpdateEventListener, IPreInsertEventListener
    {
        #region Implementation of IPreUpdateEventListener

        public Task<bool> OnPreUpdateAsync(PreUpdateEvent @event, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return true if the operation should be vetoed
        /// </summary>
        /// <param name="event"/>
        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            var entity = @event.Entity as ICreatedAndUpdatedProperty;

            if (entity == null)
                return false;

            var time = DateTime.Now;
            
            Set(@event.Persister, @event.State, "Updated", time);

            entity.Updated = time;

            if (entity.Created == DateTime.MinValue) entity.Created = time;
            
            return false;
        }

        #endregion

        #region Implementation of IPreInsertEventListener

        public Task<bool> OnPreInsertAsync(PreInsertEvent @event, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return true if the operation should be vetoed
        /// </summary>
        /// <param name="event"/>
        public bool OnPreInsert(PreInsertEvent @event)
        {
            var entity = @event.Entity as ICreatedAndUpdatedProperty;

            if (entity == null)
                return false;

            var time = (entity.Created == DateTime.MinValue ?
                DateTime.Now : entity.Created);

            Set(@event.Persister, @event.State, "Updated", time);
            Set(@event.Persister, @event.State, "Created", time);

            entity.Updated = time;
            entity.Created = time;

            return false;
        }

        #endregion

        private static void Set(IEntityPersister persister, object[] state, string propertyName, object value)
        {
            var index = Array.IndexOf(persister.PropertyNames, propertyName);
            
            if (index == -1)
                return;

            state[index] = value;
        }
    }
}