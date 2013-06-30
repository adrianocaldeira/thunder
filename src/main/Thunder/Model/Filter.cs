using System;
using System.Collections.Generic;

namespace Thunder.Model
{
    /// <summary>
    /// Filter
    /// </summary>
    public class Filter
    {
        /// <summary>
        /// Initialize new instance of classe <see cref="Filter"/>.
        /// </summary>
        public Filter()
        {
            CurrentPage = 0;
            PageSize = 15;
            Orders = new List<FilterOrder>();
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
        /// Get or set orders
        /// </summary>
        public IList<FilterOrder> Orders { get; set; }

        /// <summary>
        /// Convert current instance to type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T Is<T>()
        {
            return (T)Convert.ChangeType(this, typeof(T));
        }
    }
}
