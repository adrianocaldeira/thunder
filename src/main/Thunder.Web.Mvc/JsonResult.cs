using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Thunder.Web.Mvc
{
    /// <summary>
    /// Json result
    /// </summary>
    public class JsonResult : ActionResult
    {
        /// <summary>
        /// Initialize a new instance of the class <see cref="JsonResult"/>.
        /// </summary>
        public JsonResult()
        {
            JsonRequestBehavior = JsonRequestBehavior.DenyGet;
            Status = ResultStatus.Success;
            Messages = new List<Message>();
        }

        /// <summary>
        /// Initialize a new instance of the class <see cref="JsonResult"/>.
        /// </summary>
        /// <param name="status">Status</param>
        public JsonResult(ResultStatus status)
            : this()
        {
            Status = status;
        }

        /// <summary>
        /// Initialize a new instance of the class <see cref="JsonResult"/>.
        /// </summary>
        /// <param name="status">Status</param>
        /// <param name="modelStateDictionary">Model state dictionary</param>
        public JsonResult(ResultStatus status, ModelStateDictionary modelStateDictionary)
            : this(status)
        {
            ParseModelStateToMessage(modelStateDictionary);
        }

        ///<summary>
        /// Json result status
        ///</summary>
        public ResultStatus Status { get; set; }

        /// <summary>
        /// Get or set data
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// Get or set messages
        /// </summary>
        public List<Message> Messages { get; set; }

        /// <summary>
        /// Get or set content encoding
        /// </summary>
        public Encoding ContentEncoding { get; set; }

        /// <summary>
        /// Get an set content type
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Get or set json request behavior
        /// </summary>
        public JsonRequestBehavior JsonRequestBehavior { get; set; }

        #region Overrides of ActionResult

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
            if ((JsonRequestBehavior == JsonRequestBehavior.DenyGet) &&
                string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("HttpMethot GET not allowed.");
            }

            var response = context.HttpContext.Response;
            response.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            Serialize(context.HttpContext);
        }

        /// <summary>
        /// Serialize
        /// </summary>
        /// <param name="context"></param>
        private void Serialize(HttpContextBase context)
        {
            var json = string.Empty;
            var isCallBack = string.IsNullOrEmpty(context.Request["callback"]) ? false : true;

            if (Data == null && Messages.Count == 0)
                json = JsonConvert.SerializeObject(new { Status });
            else if (Data != null && Messages.Count == 0)
                json = JsonConvert.SerializeObject(new { Status, Data });
            else if (Data != null && Messages.Count > 0)
                json = JsonConvert.SerializeObject(new { Status, Data, Messages });
            else if (Data == null && Messages.Count > 0)
                json = JsonConvert.SerializeObject(new { Status, Messages });

            if (isCallBack)
            {
                json = string.Format("{0}({1})", context.Request["callback"], json);
            }

            context.Response.Write(json);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Parse model state dictionary to messages
        /// </summary>
        /// <param name="modelStateDictionary">Model state</param>
        private void ParseModelStateToMessage(ModelStateDictionary modelStateDictionary)
        {
            if (modelStateDictionary == null)
                throw new ArgumentNullException("modelStateDictionary");

            foreach (var key in modelStateDictionary.Keys)
            {
                foreach (var error in modelStateDictionary[key].Errors)
                {
                    Messages.Add(new Message(error.ErrorMessage, key));
                }
            }
        }

        #endregion
    }
}
