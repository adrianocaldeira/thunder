namespace Site.Models
{
    /// <summary>
    /// Filter
    /// </summary>
    public class Filter
    {
        /// <summary>
        /// Initialize new instance of class <see cref="Filter"/>.
        /// </summary>
        public Filter()
        {
            CurrentPage = 0;
            PageSize = 15;
        }

        /// <summary>
        /// Get or set current page
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Get or set page size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Get or set order
        /// </summary>
        public FilterOrder Order { get; set; }
    }
}