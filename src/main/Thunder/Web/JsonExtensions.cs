using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Thunder.Web
{
    /// <summary>
    ///     Json extensions
    /// </summary>
    public static class JsonExtensions
    {
        /// <summary>
        ///     Convert an object to a JSON string
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>JSON string</returns>
        public static string Json(this object obj)
        {
            return obj.Json(Formatting.None);
        }

        /// <summary>
        ///     Convert an object to a JSON string
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="formatting"></param>
        /// <returns></returns>
        public static string Json(this object obj, Formatting formatting)
        {
            return obj.Json(formatting, new CamelCasePropertyNamesContractResolver());
        }

        /// <summary>
        ///     Convert an object to a JSON string
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="formatting"></param>
        /// <param name="contractResolver"></param>
        /// <returns></returns>
        public static string Json(this object obj, Formatting formatting, IContractResolver contractResolver)
        {
            return obj.Json(formatting, CreateDefaultSettings(contractResolver));
        }

        /// <summary>
        ///     Convert an object to a JSON string
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="formatting"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static string Json(this object obj, Formatting formatting, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(obj, formatting, settings);
        }

        /// <summary>
        ///     Converts the specified JSON string to an object of type
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="input">Input</param>
        /// <returns>Object</returns>
        public static T Json<T>(this string input)
        {
            return JsonConvert.DeserializeObject<T>(input, CreateDefaultSettings());
        }

        /// <summary>
        ///     Cria configurações de serialização seguras, independentes de <see cref="JsonConvert.DefaultSettings" />
        ///     configurado globalmente pela aplicação host. Evita herdar <see cref="TypeNameHandling" /> perigoso
        ///     (risco de desserialização de tipos arbitrários) e limita a profundidade de aninhamento.
        /// </summary>
        /// <param name="contractResolver">Contract resolver a utilizar. Quando nulo, usa camelCase.</param>
        /// <returns>Configurações de serialização seguras</returns>
        private static JsonSerializerSettings CreateDefaultSettings(IContractResolver contractResolver = null)
        {
            return new JsonSerializerSettings
            {
                ContractResolver = contractResolver ?? new CamelCasePropertyNamesContractResolver(),
                TypeNameHandling = TypeNameHandling.None,
                MaxDepth = 64
            };
        }
    }
}