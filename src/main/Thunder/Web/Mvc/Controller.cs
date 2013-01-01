using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Thunder.Web.Mvc
{
    /// <summary>
    /// Custom controller
    /// </summary>
    public class Controller : System.Web.Mvc.Controller
    {

        /// <summary>
        /// Add message
        /// </summary>
        /// <param name="message">Message</param>
        public void AddMessage(string message)
        {
            AddMessage(new Message(message));
        }

        /// <summary>
        /// Add message
        /// </summary>
        /// <param name="message"></param>
        public void AddMessage(Message message)
        {

            if (Session[Constants.ThunderSessionMessage] == null)
                Session[Constants.ThunderSessionMessage] = new List<Message>();

            var messages = (IList<Message>)Session[Constants.ThunderSessionMessage];

            messages.Add(message);
        }

        /// <summary>
        ///  Create view result 
        /// </summary>
        /// <param name="status">Status</param>
        /// <returns>Message result</returns>
        public MessageResult View(ResultStatus status)
        {
            return View(status, false);
        }

        /// <summary>
        /// Create view result 
        /// </summary>
        /// <param name="status">Status</param>
        /// <param name="showCloseButton">Show close button</param>
        /// <returns>Message result</returns>
        public MessageResult View(ResultStatus status, bool showCloseButton)
        {
            return new MessageResult { Status = status, ShowCloseButton = showCloseButton };
        }

        /// <summary>
        /// Create view result 
        /// </summary>
        /// <param name="status">Result status</param>
        /// <param name="message">Message</param>
        /// <returns>Message result</returns>
        public MessageResult View(ResultStatus status, Message message)
        {
            return View(status, message, false);
        }

        /// <summary>
        /// Create view result 
        /// </summary>
        /// <param name="status">Result status</param>
        /// <param name="message">Message</param>
        /// <param name="showCloseButton">Show close button</param>
        /// <returns>Message result</returns>
        public MessageResult View(ResultStatus status, Message message, bool showCloseButton)
        {
            return new MessageResult(status, message) { ShowCloseButton = showCloseButton };
        }

        /// <summary>
        /// Create view result
        /// </summary>
        /// <param name="status">Result status</param>
        /// <param name="message">Message</param>
        /// <returns>Message result</returns>
        public MessageResult View(ResultStatus status, string message)
        {
            return View(status, message, false);
        }

        /// <summary>
        /// Create view result
        /// </summary>
        /// <param name="status">Result status</param>
        /// <param name="message">Message</param>
        /// <param name="showCloseButton">Show close button</param>
        /// <returns>Message result</returns>
        public MessageResult View(ResultStatus status, string message, bool showCloseButton)
        {
            return new MessageResult(status, message) { ShowCloseButton = showCloseButton };
        }

        /// <summary>
        /// Create view result
        /// </summary>
        /// <param name="status">Status</param>
        /// <param name="messages">Messages</param>
        /// <returns>Message result</returns>
        public MessageResult View(ResultStatus status, IList<Message> messages)
        {
            return View(status, messages, false);
        }

        /// <summary>
        /// Create view result
        /// </summary>
        /// <param name="status">Status</param>
        /// <param name="messages">Messages</param>
        /// <param name="showCloseButton">Show close button</param>
        /// <returns>Message result</returns>
        public MessageResult View(ResultStatus status, IList<Message> messages, bool showCloseButton)
        {
            return new MessageResult { Status = status, Messages = messages, ShowCloseButton = showCloseButton};
        }

        /// <summary>
        /// Get messages
        /// </summary>
        public IList<Message> Messages
        {
            get { return Session[Constants.ThunderSessionMessage] as IList<Message> ?? new List<Message>(); }
        }

        /// <summary>
        /// Called before the action method is invoked
        /// </summary>
        /// <param name="filterContext">Filter context</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Messages.Count > 0)
            {
                ViewBag.ShowClose = true;
                ViewBag.Messages = new Model { Messages = Messages.ToList(), Status = ResultStatus.Success };
                Messages.Clear();
            }

            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// Render partial view
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns>View</returns>
        protected string RenderPartial(object model)
        {
            return RenderPartial(null, model);
        }

        /// <summary>
        /// Render partial view
        /// </summary>
        /// <param name="viewName">View name</param>
        /// <param name="model">Model</param>
        /// <returns>View</returns>
        protected string RenderPartial(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();
            }
        }

        /// <summary>
        /// Exclude properties in validation of model state
        /// </summary>
        /// <param name="properties">Properties</param>
        public void ExcludePropertiesInValidation(params string[] properties)
        {
            if (ModelState.IsValid) return;

            foreach (var property in properties)
            {
                if(ModelState.ContainsKey(property))
                {
                    ModelState.Remove(property);                    
                }
            }
        }

        /// <summary>
        /// OnException
        /// </summary>
        /// <param name="filterContext"><see cref="ExceptionContext"/></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            Logging.Write.Error(filterContext.Exception);

            base.OnException(filterContext);
        }

        /// <summary>
        /// Get visitor IP address
        /// </summary>
        public string Visitor
        {
            get
            {
                return Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];
            }
        }
    }
}