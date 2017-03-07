using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Thunder.Web
{
    /// <summary>
    /// Json extensions
    /// </summary>
    public static class JsonExtensions
    {
        /// <summary>
        /// Convert an object to a JSON string
        /// </summary>
        /// <param name="obj">Object</param>
        /// <param name="formatting"><see cref="Formatting"/></param>
        /// <returns>JSON string</returns>
        public static string Json(this object obj, Formatting formatting = Formatting.Indented)
        {
            return JsonConvert.SerializeObject(obj, formatting,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
        }

        /// <summary>
        /// Converts the specified JSON string to an object of type
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="input">Input</param>
        /// <returns>Object</returns>
        public static T Json<T>(this string input)
        {
            return JsonConvert.DeserializeObject<T>(input);
        }
    }
}
