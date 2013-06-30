using Thunder.Model;

namespace Thunder.Collections
{
    /// <summary>
    /// Paging filter interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPagingFilter<T> : IPaging<T>
    {
        /// <summary>
        /// Get or set filter
        /// </summary>
        Filter Filter { get; set; }
    }
}