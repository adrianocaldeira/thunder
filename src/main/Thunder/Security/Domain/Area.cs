using System.Collections.Generic;

namespace Thunder.Security.Domain
{
    /// <summary>
    /// Area
    /// </summary>
    public class Area : IArea
    {
        /// <summary>
        /// Initialize a new instance of the class <see cref="Area"/>.
        /// </summary>
        public Area()
        {
            Childs = new List<IArea>();
        }

        #region IArea Members

        /// <summary>
        /// Get or set area id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Get or set area module
        /// </summary>
        public virtual IArea Parent { get; set; }

        /// <summary>
        /// Get or set area name
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Get or set area url
        /// </summary>
        public virtual string Url { get; set; }

        /// <summary>
        /// Get or set area childs
        /// </summary>
        public virtual IList<IArea> Childs { get; set; }

        #endregion
    }
}