namespace Manager.Models
{
    ///<summary>
    /// Filter Order
    ///</summary>
    public class FilterOrder
    {
        /// <summary>
        /// Initialize new instance of class <see cref="FilterOrder"/>.
        /// </summary>
        public FilterOrder()
        {
            Asc = true;
        }

        ///<summary>
        /// Get or set property
        ///</summary>
        public string Property { get; set; }

        /// <summary>
        /// Get or set order asc
        /// </summary>
        public bool Asc { get; set; }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} {1}", Property, (Asc ? "asc" : "desc"));
        }
    }
}