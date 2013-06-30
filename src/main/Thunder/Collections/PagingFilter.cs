using System;
using System.Collections.Generic;
using System.Linq;
using Thunder.Model;

namespace Thunder.Collections
{
    /// <summary>
    /// Paging with filter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagingFilter<T> : List<T>, IPagingFilter<T>
    {
        #region Constructors

        /// <summary>
        /// Initialize new instance of <see cref="Paging{T}"/>.
        /// </summary>
        /// <param name="source">Source data</param>
        /// <param name="filter">Filter</param>
        /// <param name="records">Total records</param>
        public PagingFilter(IEnumerable<T> source, Filter filter, long? records = null) :
            this(source.AsQueryable(), filter, records)
        {
        }

        /// <summary>
        /// Initialize new instance of <see cref="Paging{T}"/>.
        /// </summary>
        /// <param name="source">Source data</param>
        /// <param name="filter">Filter</param>
        /// <param name="records">Total records</param>
        public PagingFilter(IQueryable<T> source, Filter filter, long? records = null)
        {
            if (filter.CurrentPage < 0)
            {
                throw new ArgumentOutOfRangeException("currentPage", "Value can not be below 0.");
            }

            if (filter.PageSize < 1)
            {
                throw new ArgumentOutOfRangeException("pageSize", "Value can not be less than 1.");
            }

            if (source == null)
            {
                source = new List<T>().AsQueryable();
            }

            PageSize = filter.PageSize;
            CurrentPage = filter.CurrentPage;
            Records = records.HasValue ? records.Value : source.Count();

            if (Records <= 0) return;

            SetSkip(source);

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

        /// <summary>
        /// Get or set filter
        /// </summary>
        public Filter Filter { get; set; }
        #endregion

        #region Private methods

        /// <summary>
        /// Set skip records
        /// </summary>
        /// <param name="source">Source</param>
        private void SetSkip(IQueryable<T> source)
        {
            var records = source.Count();
            var pageCount = (int) Math.Ceiling(records/(double) PageSize);

            if (records < Records && pageCount <= CurrentPage)
                Skip = ((pageCount - 1)*PageSize);
            else
                Skip = (CurrentPage*PageSize);
        }
        #endregion
    }
}