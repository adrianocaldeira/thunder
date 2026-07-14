using System.Text.RegularExpressions;

namespace Thunder.Web.Mvc.Internal
{
    /// <summary>
    /// Validate JSONP callback names before echoing them back into the response,
    /// preventing reflected script injection via the callback query string value.
    /// </summary>
    public static class JsonpCallback
    {
        private static readonly Regex ValidCallback = new Regex(@"^[A-Za-z_$][A-Za-z0-9_$.\[\]]*$", RegexOptions.Compiled);

        /// <summary>
        /// Determine whether <paramref name="callback"/> is a safe JSONP callback name.
        /// </summary>
        /// <param name="callback">Callback name informed by the caller (e.g. querystring "callback").</param>
        /// <returns><c>true</c> when the callback is non-empty and matches a safe identifier pattern.</returns>
        public static bool IsValid(string callback)
        {
            return !string.IsNullOrEmpty(callback) && ValidCallback.IsMatch(callback);
        }
    }
}
