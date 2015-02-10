using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Thunder.Web.Mvc.Extensions
{
    /// <summary>
    ///     ModelState Dictionary Extensions
    /// </summary>
    public static class ModelStateDictionaryExtensions
    {
        /// <summary>
        ///     Convert <see cref="ModelStateDictionary" /> to list of key and value
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, IList<string>>> ToKeyAndValues(this ModelStateDictionary source)
        {
            return (from key in source.Keys
                where source[key].Errors.Any()
                select
                    new KeyValuePair<string, IList<string>>(key, source[key].Errors.Select(error => error.ErrorMessage)
                        .ToList()));
        }

        /// <summary>
        ///     Convert dictionary <see cref="ModelState" /> to list of key and value
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, IList<string>>> ToKeyAndValues(this IDictionary<string, ModelState> source)
        {
            return (from key in source.Keys
                    where source[key].Errors.Any()
                    select
                        new KeyValuePair<string, IList<string>>(key, source[key].Errors.Select(error => error.ErrorMessage)
                            .ToList()));
        }
    }
}