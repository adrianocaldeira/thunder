using Newtonsoft.Json;

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
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Json(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
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
