using System;

namespace Thunder.Data
{
    /// <summary>
    /// Persist object
    /// </summary>
    /// <typeparam name="T">Type class</typeparam>
    /// <typeparam name="TKey">Type key</typeparam>
    public class Persist<T, TKey> where T : class
    {
        /// <summary>
        /// Get or set id
        /// </summary>
        public virtual TKey Id { get; set; }

        /// <summary>
        /// Get or set created date
        /// </summary>
        public virtual DateTime Created { get; set; }

        /// <summary>
        /// Get or set updated date
        /// </summary>
        public virtual DateTime Updated { get; set; }

         /// <summary>
        /// Is new object
        /// </summary>
        /// <returns></returns>
        public virtual bool IsNew()
        {
            var id = Convert.ChangeType(Id, TypeCode.Int64);
            return id == null || (Int64)id <= 0;
        }

        /// <summary>
        /// Notify updated object
        /// </summary>
        public virtual void NotifyUpdated()
        {
            Updated = DateTime.Now;
        }

        /// <summary>
        /// Notify created object
        /// </summary>
        public virtual void NotifyCreated()
        {
            Created = DateTime.Now;
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var compare = obj as T;

            if (compare == null)
            {
                return false;
            }

            return (GetHashCode() == compare.GetHashCode());
        }

        /// <summary>
        /// GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (29 * Id.GetHashCode());
            }
        }
    }
}
