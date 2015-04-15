using System;
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

        /// <summary>
        /// Exclude properties with key part in validation of model state
        /// </summary>
        /// <param name="source"><see cref="ModelStateDictionary"/></param>
        /// <param name="keyPart">Key part in validation of model state</param>
        /// <param name="ignoreKeys">Ignore keys</param>
        public static void ExcludePropertiesWithKeyPart(this ModelStateDictionary source, string keyPart, string[] ignoreKeys)
        {
            if (source.IsValid) return;

            foreach (var item in source.ToList().Where(item => 
                item.Key.ToLower().IndexOf(keyPart.ToLower(), StringComparison.InvariantCulture) != -1
                && (ignoreKeys != null && !ignoreKeys.Contains(item.Key))))
            {
                source.Remove(item);
            }
        }

        /// <summary>
        /// Add model error with format
        /// </summary>
        /// <param name="source"><see cref="ModelStateDictionary"/></param>
        /// <param name="key">Key</param>
        /// <param name="errorMessage">Error message</param>
        /// <param name="args">Replace de format parameters</param>
        public static void AddModelErrorFormat(this ModelStateDictionary source, string key, string errorMessage, params object[] args)
        {
            source.AddModelError(key, string.Format(errorMessage, args));
        }

        /// <summary>
        /// Cast for list of string
        /// </summary>
        /// <param name="source"><see cref="ModelStateDictionary"/></param>
        /// <returns>List of string</returns>
        public static IList<string> ToListOfString(this ModelStateDictionary source)
        {
            return (from key in source.Keys where source[key].Errors.Any() from error in source[key].Errors select error.ErrorMessage).ToList();
        }
    }
}