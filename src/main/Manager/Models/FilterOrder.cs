namespace Manager.Models
{
    ///<summary>
    /// Ordem de filtro
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
        /// Recupera ou define propriedade
        ///</summary>
        public string Property { get; set; }

        /// <summary>
        /// Recupera ou define tipo da ordem
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