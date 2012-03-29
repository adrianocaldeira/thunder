using System.Collections.Generic;

namespace Thunder.Security.Domain
{
    /// <summary>
    /// Area interface
    /// </summary>
    public interface IArea
    {
        /// <summary>
        /// Get or set area id
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Get or set parent area
        /// </summary>
        IArea Parent { get; set; }

        /// <summary>
        /// Get or set area name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Get or set area url
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// Get or set area childs
        /// </summary>
        IList<IArea> Childs { get; set; }
    }
}