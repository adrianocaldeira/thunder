namespace $rootnamespace$.Models
{
    /// <summary>
    /// Filtro
    /// </summary>
    public class Filter
    {
        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="Filter"/>.
        /// </summary>
        public Filter()
        {
            CurrentPage = 0;
            PageSize = 15;
        }

        /// <summary>
        /// Recupera ou define página corrente
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Recupera ou define tamanho da página
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Recupera ou define ordem
        /// </summary>
        public FilterOrder Order { get; set; }
    }
}