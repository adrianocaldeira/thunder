using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Thunder.Web.Mvc
{
    /// <summary>
    /// Message result
    /// </summary>
    [Obsolete("No use this class, it will be removed in future")]
    public class MessageResult : ActionResult
    {
        /// <summary>
        /// Initialize new instance of class <see cref="MessageResult"/>
        /// </summary>
        public MessageResult()
        {
            Status = ResultStatus.Success;
            Messages = new List<Message>();
        }

        /// <summary>
        /// Initialize new instance of class <see cref="MessageResult"/>
        /// </summary>
        /// <param name="message">Message</param>
        public MessageResult(Message message) : this(ResultStatus.Success, message)
        {
        }

        /// <summary>
        /// Initialize new instance of class <see cref="MessageResult"/>
        /// </summary>
        /// <param name="status">Result status</param>
        /// <param name="message">Message</param>
        public MessageResult(ResultStatus status, Message message) : this()
        {
            Status = status;
            Messages.Add(message);
        }

        /// <summary>
        /// Initialize new instance of class <see cref="MessageResult"/>
        /// </summary>
        /// <param name="message">Message</param>
        public MessageResult(string message) : this(ResultStatus.Success, new Message(message))
        {
        }

        /// <summary>
        /// Initialize new instance of class <see cref="MessageResult"/>
        /// </summary>
        /// <param name="status">Result status</param>
        /// <param name="message">Message</param>
        public MessageResult(ResultStatus status, string message) : this(status, new Message(message))
        {
        }

        /// <summary>
        /// Get or set result status
        /// </summary>
        public ResultStatus Status { get; set; }

        /// <summary>
        /// Get or set messages
        /// </summary>
        public IList<Message> Messages { get; set; }

        /// <summary>
        /// Get or set if show close button
        /// </summary>
        public bool ShowCloseButton { get; set; }

        /// <summary>
        /// Get css class
        /// </summary>
        private string CssClass
        {
            get
            {
                if (Status == ResultStatus.Success)
                {
                    return "thunder-success";
                }

                if (Status == ResultStatus.Error)
                {
                    return "thunder-error";
                }

                if (Status == ResultStatus.Information)
                {
                    return "thunder-information";
                }

                return "thunder-attention";
            }
        }

        /// <summary>
        /// Execute result
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            var controller = (System.Web.Mvc.Controller) context.Controller;

            if (!controller.ModelState.IsValid)
            {
                Messages = (from key in controller.ModelState.Keys
                            from error
                                in controller.ModelState[key].Errors
                            select new Message(error.ErrorMessage, key)).ToList();
            }

            Execute(context);
        }

        private void Execute(ControllerContext context)
        {
            if (Messages.Count.Equals(0))
                return;

            var html = new StringBuilder();
            string id = Guid.NewGuid().ToString();

            html.AppendLine(string.Format("<div id=\"{0}\" class=\"thunder-notification {1}\">", id, CssClass));

            #region Close Button

            if (ShowCloseButton)
            {
                html.AppendLine("<a class=\"close\" href=\"#\"></a>");
            }

            #endregion

            #region Content

            html.AppendLine("<div class=\"thunder-notification-content\">");

            if (Messages.Count.Equals(1))
            {
                html.AppendLine(Messages[0].Text);
            }
            else
            {
                html.AppendLine("<ul>");
                foreach (var message in Messages)
                {
                    html.AppendLine(string.Format("<li>{0}</li>", message.Text));
                }
                html.AppendLine("</ul>");
            }

            html.AppendLine("</div>");

            #endregion

            html.AppendLine("</div>");

            #region Script

            if (ShowCloseButton)
            {
                html.AppendLine("<script type=\"text/javascript\">");
                html.AppendLine("$(function () {");
                html.AppendLine("window.setTimeout(function () {");
                html.AppendLine(string.Format("$('#{0}').slideUp();", id));
                html.AppendLine("}, 5000);");
                html.AppendLine("});");
                html.AppendLine("</script>");
            }

            #endregion

            context.RequestContext.HttpContext.Response.Write(html.ToString());
        }
    }
}