using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Thunder.Web.Mvc
{
    /// <summary>
    /// Json Result
    /// </summary>
    public class JsonResult : System.Web.Mvc.JsonResult
    {
        /// <summary>
        ///     Initialize new instance
        /// </summary>
        public JsonResult()
        {
            Settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        /// <summary>
        ///     Get or set <see cref="JsonSerializerSettings" />
        /// </summary>
        public JsonSerializerSettings Settings { get; set; }

        /// <summary>
        /// Get or set data object
        /// </summary>
        public new object Data { get; set; }

        /// <summary>
        /// Get or set type json result
        /// </summary>
        public JsonResultType Type { get; set; }

        /// <summary>
        /// Execute result
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if ((JsonRequestBehavior == JsonRequestBehavior.DenyGet) && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("HttpMethot GET not allowed.");
            }

            var response = context.HttpContext.Response;
            var json = JsonConvert.SerializeObject(new { Type, Data },
                Formatting.Indented, Settings);
            
            response.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

            if (ContentEncoding != null) response.ContentEncoding = ContentEncoding;

            if (!string.IsNullOrEmpty(context.HttpContext.Request["callback"]))
            {
                json = string.Format("{0}({1})", context.HttpContext.Request["callback"], json);
            }

            context.HttpContext.Response.Write(json);

            base.ExecuteResult(context);
        }
    }
}
