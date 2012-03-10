using System.Collections.Generic;

namespace Thunder.Data
{
    /// <summary>
    /// Collection paging
    /// </summary>
    public class Collection<T>
    {
        /// <summary>
        /// Initialize new instance of <see cref="Collection{T}"/>.
        /// </summary>
        /// <param name="data"></param>
        public Collection(IPaging<T> data)
        {
            PageCount = data.PageCount;
            Records = data.Records;
            PageSize = data.PageSize;
            IsFirstPage = data.IsFirstPage;
            IsLastPage = data.IsLastPage;
            Rows = data;
            CurrentPage = data.CurrentPage;
        }

        /// <summary>
        /// Get or set current page
        /// </summary>
        public int CurrentPage { get; private set; }

        /// <summary>
        ///Get or set rows
        /// </summary>
        public IList<T> Rows { get; private set; }

        /// <summary>
        /// Get or set page count
        /// </summary>
        public int PageCount { get; private set; }

        /// <summary>
        /// Get or set records
        /// </summary>
        public long Records { get; private set; }

        /// <summary>
        /// Get or set page size
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// Get or set is firt page
        /// </summary>
        public bool IsFirstPage { get; private set; }

        /// <summary>
        /// Get or set is last page
        /// </summary>
        public bool IsLastPage { get; private set; }
    }
}