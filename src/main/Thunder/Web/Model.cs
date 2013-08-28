using System;
using System.Collections.Generic;

namespace Thunder.Web
{
    /// <summary>
    /// Model data
    /// </summary>
    [Obsolete("No use this property, it will be removed in future")]
    public class Model : IModel
    {
        #region IModel Members

        /// <summary>
        /// Get or set status of result
        /// </summary>
        public ResultStatus Status { get; set; }

        /// <summary>
        /// Get or set messages collection
        /// </summary>
        public IList<Message> Messages { get; set; }

        #endregion
    }
}