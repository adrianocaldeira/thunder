namespace Thunder.Extensions
{
    /// <summary>
    /// Boolean extensions
    /// </summary>
    public static class BooleanExtensions
    {
        /// <summary>
        /// Transforme booelan to text plan Sim or Não 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Text(this bool source)
        {
            return source ? "Sim" : "Não";
        }
    }
}