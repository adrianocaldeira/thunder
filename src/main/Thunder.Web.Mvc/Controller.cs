using System;
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
        /// Get messages
        /// </summary>
        [Obsolete("No use this property, it will be removed in future")]
        public IList<Message> Messages
        {
            get { return Session[Constants.ThunderSessionMessage] as IList<Message> ?? new List<Message>(); }
        }

        /// <summary>
        /// Add message
        /// </summary>
        /// <param name="message">Message</param>
        [Obsolete("No use this method, it will be removed in future")]
        public void AddMessage(string message)
        {
            AddMessage(new Message(message));
        }

        /// <summary>
        /// Add message
        /// </summary>
        /// <param name="message"></param>
        [Obsolete("No use this method, it will be removed in future")]
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
        [Obsolete("No use this method, it will be removed in future")]
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
        [Obsolete("No use this method, it will be removed in future")]
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
        [Obsolete("No use this method, it will be removed in future")]
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
        [Obsolete("No use this method, it will be removed in future")]
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
        [Obsolete("No use this method, it will be removed in future")]
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
        [Obsolete("No use this method, it will be removed in future")]
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
        [Obsolete("No use this method, it will be removed in future")]
        public MessageResult View(ResultStatus status, IList<Message> messages, bool showCloseButton)
        {
            return new MessageResult { Status = status, Messages = messages, ShowCloseButton = showCloseButton};
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

            ViewData[Constants.ViewData.Notify] = Session[Constants.ViewData.Notify] as Notify ?? new Notify();
            
            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// Set notify
        /// </summary>
        /// <param name="notify"><see>
        ///                        <cref>Notify</cref>
        ///                      </see> </param>
        public void SetNotify(Notify notify)
        {
            Session[Constants.ViewData.Notify] = notify ?? new Notify();
        }

        /// <summary>
        /// Notify result
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns><see cref="NotifyResult"/></returns>
        public NotifyResult Notify(string message)
        {
            return Notify(new List<string> {message});
        }

        /// <summary>
        /// Notify result
        /// </summary>
        /// <param name="messages">Message</param>
        /// <returns><see cref="NotifyResult"/></returns>
        public NotifyResult Notify(IList<string> messages)
        {
            return Notify(NotifyType.Success, messages);
        }

        /// <summary>
        /// Notify result
        /// </summary>
        /// <param name="notifyType"><see cref="NotifyType"/></param>
        /// <param name="message">Message</param>
        /// <returns><see cref="NotifyResult"/></returns>
        public NotifyResult Notify(NotifyType notifyType, string message)
        {
            return Notify(notifyType, new List<string> {message});
        }

        /// <summary>
        /// Notify result
        /// </summary>
        /// <param name="notifyType"><see cref="NotifyType"/></param>
        /// <param name="messages">Messages</param>
        /// <returns><see cref="NotifyResult"/></returns>
        public NotifyResult Notify(NotifyType notifyType, IList<string> messages)
        {
            return new NotifyResult(notifyType, messages);
        }

        /// <summary>
        /// Notify result
        /// </summary>
        /// <param name="notifyType"><see cref="NotifyType"/></param>
        /// <param name="modelStates"><see cref="IDictionary{TKey,TValue}"/></param>
        /// <returns><see cref="NotifyResult"/></returns>
        public NotifyResult Notify(NotifyType notifyType, IDictionary<string, ModelState> modelStates)
        {
            return new NotifyResult(notifyType, modelStates);
        }

        /// <summary>
        /// Notify result
        /// </summary>
        /// <param name="notify"><see>
        ///                        <cref>Notify</cref>
        ///                      </see> </param>
        /// <returns><see cref="NotifyResult"/></returns>
        public NotifyResult Notify(Notify notify)
        {
            return new NotifyResult(notify);
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