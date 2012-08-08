using System.Collections.Generic;

namespace Thunder.Collections
{
    /// <summary>
    /// Paging interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPaging<T> : IList<T>
    {
        /// <summary>
        /// Get total pages
        /// </summary>
        int PageCount { get; }

        /// <summary>
        /// Get total records 
        /// </summary>
        long Records { get; }

        /// <summary>
        /// Get current page
        /// </summary>
        int CurrentPage { get; }

        /// <summary>
        /// Get page size
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// Get information if there is previous page
        /// </summary>
        bool HasPreviousPage { get; }

        /// <summary>
        /// Get information if there is next page
        /// </summary>
        bool HasNextPage { get; }

        /// <summary>
        /// Get information if is first page
        /// </summary>
        bool IsFirstPage { get; }

        /// <summary>
        /// Get information if is last page
        /// </summary>
        bool IsLastPage { get; }

        /// <summary>
        /// Get skip records
        /// </summary>
        int Skip { get; }
    }
}
