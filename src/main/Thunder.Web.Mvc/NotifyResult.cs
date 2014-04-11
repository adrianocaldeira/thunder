using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Thunder.Extensions;

namespace Thunder.Web.Mvc
{
    /// <summary>
    /// Notify result
    /// </summary>
    public class NotifyResult : ActionResult
    {
        /// <summary>
        /// Initialize new instance of class <see cref="NotifyResult"/>.
        /// </summary>
        public NotifyResult()
        {
            Type = NotifyType.Success;
            Messages = new List<KeyValuePair<string, IList<string>>>();
            JsonRequestBehavior = JsonRequestBehavior.DenyGet;
        }

        /// <summary>
        /// Initialize new instance of class <see cref="NotifyResult"/>.
        /// </summary>
        /// <param name="message">Message</param>
        public NotifyResult(string message)
            : this(NotifyType.Success, message)
        {

        }

        /// <summary>
        /// Initialize new instance of class <see cref="NotifyResult"/>.
        /// </summary>
        /// <param name="type"><see cref="NotifyType"/></param>
        /// <param name="message">Message</param>
        public NotifyResult(NotifyType type, string message)
            : this(type, new List<string> { message })
        {

        }

        /// <summary>
        /// Initialize new instance of class <see cref="NotifyResult"/>.
        /// </summary>
        /// <param name="messages">Messages</param>
        public NotifyResult(IEnumerable<string> messages)
            : this(NotifyType.Success, messages)
        {

        }

        /// <summary>
        /// Initialize new instance of class <see cref="NotifyResult"/>.
        /// </summary>
        /// <param name="type"><see cref="NotifyType"/></param>
        /// <param name="messages">Messages</param>
        public NotifyResult(NotifyType type, IEnumerable<string> messages) : this()
        {
            Type = type;
            Messages = messages.Select(message => new KeyValuePair<string, IList<string>>(Guid.NewGuid().ToString(), new List<string> { message })).ToList();
        }

        /// <summary>
        /// Initialize new instance of class <see cref="NotifyResult"/>.
        /// </summary>
        /// <param name="notify"><see cref="Notify"/></param>
        public NotifyResult(Notify notify)
        {
            Type = notify.Type;
            Messages = notify.Messages.Select(message => new KeyValuePair<string, IList<string>>(Guid.NewGuid().ToString(), new List<string> { message })).ToList(); 
        }

        /// <summary>
        /// Initialize new instance of class <see cref="NotifyResult"/>.
        /// </summary>
        /// <param name="type"><see cref="NotifyType"/></param>
        /// <param name="modelState"><see cref="IDictionary{TKey,TValue}"/></param>
        public NotifyResult(NotifyType type, IDictionary<string, ModelState> modelState)
            : this()
        {
            Type = type;

            Messages = (from key in modelState.Keys where modelState[key].Errors.Any() 
                        select new KeyValuePair<string, IList<string>>(key, modelState[key].Errors.Select(error => error.ErrorMessage).ToList())
            ).ToList(); 
        }

        /// <summary>
        /// Get or set type
        /// </summary>
        public NotifyType Type { get; private set; }

        /// <summary>
        /// Get or set messages
        /// </summary>
        public IList<KeyValuePair<string, IList<string>>> Messages { get; private set; }

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

        /// <summary>
        /// Get or set data result
        /// </summary>
        public object Data { get; set; }

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

            if(context.HttpContext.Request.IsAjaxRequest())
            {
                Ajax(context);
            }
            else
            {
                Html(context);
            }
        }

        private void Ajax(ControllerContext context)
        {
            if ((JsonRequestBehavior == JsonRequestBehavior.DenyGet) &&
                string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("HttpMethot GET not allowed.");
            }

            var response = context.HttpContext.Response;
            var json = JsonConvert.SerializeObject(new { Type, Messages },
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

            response.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            if (!string.IsNullOrEmpty(context.HttpContext.Request["callback"]))
            {
                json = string.Format("{0}({1})", context.HttpContext.Request["callback"], json);
            }

            context.HttpContext.Response.Write(json);
        }

        private void Html(ControllerContext context)
        {
            var builder = new StringBuilder();
            var messages = new StringBuilder();

           if(Messages.Count > 0)
           {
               if(Messages.Count == 1)
               {
                   messages.Append(Messages[0]);
               }
               else
               {
                   messages.Append("<ul>");
                   foreach (var message in Messages)
                   {
                       messages.Append("<li>{0}</li>".With(message));
                   }
                   messages.Append("</ul>");
               }
           }

            builder.Append("<div class=\"alert{0}\">".With(CssClass()))
                .Append("<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">&times;</button>")
                .Append(messages)
                .Append("</div>");

            context.HttpContext.Response.Write(builder.ToString());
        }

        private string CssClass()
        {
            var cssClass = string.Empty;

            switch (Type)
            {
                case NotifyType.Danger:
                    cssClass = " alert-danger";
                    break;
                case NotifyType.Information:
                    cssClass = " alert-info";
                    break;
                case NotifyType.Success:
                    cssClass = " alert-success";
                    break;
                case NotifyType.Warning:
                    cssClass = " alert-warning";
                    break;
            }

            return cssClass;
        }
    }
}