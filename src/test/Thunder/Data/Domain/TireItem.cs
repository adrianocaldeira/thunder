namespace Thunder.Data.Domain
{
    public class TireItem : ActiveRecord<TireItem, int>, IObjectState
    {
        public virtual string Name { get; set; }
        public virtual string Type { get; set; }

        #region Implementation of IObjectState

        /// <summary>
        /// Get or set state object
        /// </summary>
        public ObjectState State { get; set; }

        #endregion
    }
}
