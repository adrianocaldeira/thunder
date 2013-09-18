using System;

namespace Thunder.Web
{
    /// <summary>
    /// Result status
    /// </summary>
    [Obsolete("No use this enum, it will be removed in future")]
    public enum ResultStatus
    {
        /// <summary>
        /// Success
        /// </summary>
        Success = 200,
        /// <summary>
        /// Unauthorized
        /// </summary>
        Unauthorized,
        /// <summary>
        /// Error
        /// </summary>
        Error,
        /// <summary>
        /// Information
        /// </summary>
        Information,
        /// <summary>
        /// Attention
        /// </summary>
        Attention,
        /// <summary>
        /// Not connected
        /// </summary>
        NotConnected
    }
}
