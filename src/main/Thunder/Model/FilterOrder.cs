namespace Thunder.Model
{
    ///<summary>
    /// Filter order
    ///</summary>
    public class FilterOrder
    {
        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="FilterOrder"/>.
        /// </summary>
        public FilterOrder()
        {
            Asc = true;
        }

        ///<summary>
        /// Get or set column for sort
        ///</summary>
        public string Column { get; set; }

        /// <summary>
        /// Get or set sort type
        /// </summary>
        public bool Asc { get; set; }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} {1}", Column, (Asc ? "asc" : "desc"));
        }
    }
}
