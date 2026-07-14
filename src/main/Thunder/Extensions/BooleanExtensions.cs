namespace Thunder.Extensions
{
    /// <summary>
    /// Boolean extensions
    /// </summary>
    public static class BooleanExtensions
    {
        /// <summary>
        /// Transforma boolean em texto Sim ou Não
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Text(this bool source)
        {
            return source ? "Sim" : "Não";
        }
    }
}