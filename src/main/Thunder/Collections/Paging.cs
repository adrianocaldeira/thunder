using System;
using System.Collections.Generic;
using System.Linq;

namespace Thunder.Collections
{
    ///<summary>
    /// Paging
    ///</summary>
    ///<typeparam name="T">Type</typeparam>
    public class Paging<T> : List<T>, IPaging<T>
    {
        #region Constructors

        /// <summary>
        /// Initialize new instance of <see cref="Paging{T}"/>.
        /// </summary>
        /// <param name="source">Source data</param>
        /// <param name="currentPage">Current page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="records">Total records</param>
        public Paging(IEnumerable<T> source, int currentPage, int pageSize, long? records = null) :
            this(source.AsQueryable(), currentPage, pageSize, records)
        {
        }

        /// <summary>
        /// Initialize new instance of <see cref="Paging{T}"/>.
        /// </summary>
        /// <param name="source">Source data</param>
        /// <param name="currentPage">Current page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="records">Total records</param>
        public Paging(IQueryable<T> source, int currentPage, int pageSize, long? records = null)
        {
            if (currentPage < 0)
            {
                throw new ArgumentOutOfRangeException("currentPage", "Value can not be below 0.");
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException("pageSize", "Value can not be less than 1.");
            }

            if (source == null)
            {
                source = new List<T>().AsQueryable();
            }

            PageSize = pageSize;
            CurrentPage = currentPage;
            Records = records ?? source.Count();

            if (Records <= 0) return;

            SetSkip(Records);

            AddRange(source.Skip(Skip).Take(PageSize));
        }

        #endregion

        #region IPaging<T> Members

        /// <summary>
        /// Get total pages
        /// </summary>
        public int PageCount
        {
            get { return Records > 0 ? (int) Math.Ceiling(Records/(double) PageSize) : 0; }
        }

        /// <summary>
        /// Get total records 
        /// </summary>
        public long Records { get; private set; }

        /// <summary>
        /// Get current page
        /// </summary>
        public int CurrentPage { get; private set; }

        /// <summary>
        /// Get page size
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// Get information if there is previous page
        /// </summary>
        public bool HasPreviousPage
        {
            get { return (CurrentPage > 0); }
        }

        /// <summary>
        /// Get information if there is next page
        /// </summary>
        public bool HasNextPage
        {
            get { return (CurrentPage < (PageCount - 1)); }
        }

        /// <summary>
        /// Get information if is first page
        /// </summary>
        public bool IsFirstPage
        {
            get { return (CurrentPage <= 0); }
        }

        /// <summary>
        /// Get information if is last page
        /// </summary>
        public bool IsLastPage
        {
            get { return (CurrentPage >= (PageCount - 1)); }
        }

        /// <summary>
        /// Get skip records
        /// </summary>
        public int Skip { get; private set; }

        #endregion

        #region Private methods

        /// <summary>
        /// Set skip records
        /// </summary>
        /// <param name="records"></param>
        private void SetSkip(long records)
        {
            var pageCount = (int) Math.Ceiling(records/(double) PageSize);

            if (records < Records && pageCount <= CurrentPage)
                Skip = ((pageCount - 1)*PageSize);
            else
                Skip = (CurrentPage*PageSize);
        }

        #endregion
    }
}