using System.Collections.Generic;

namespace Thunder.Web
{
    /// <summary>
    /// Interface of view model
    /// </summary>
    public interface IModel
    {
        /// <summary>
        /// Get or set status of result
        /// </summary>
        ResultStatus Status { get; set; }

        /// <summary>
        /// Get or set messages collection
        /// </summary>
        IList<Message> Messages { get; set; }
    }
}