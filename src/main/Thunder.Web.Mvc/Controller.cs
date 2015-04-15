using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Thunder.Web.Mvc.Extensions;

namespace Thunder.Web.Mvc
{
    /// <summary>
    ///     Custom controller
    /// </summary>
    public class Controller : System.Web.Mvc.Controller
    {
        /// <summary>
        ///     Get visitor IP address
        /// </summary>
        public string Ip
        {
            get { return Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"]; }
        }

        /// <summary>
        ///     Called before the action method is invoked
        /// </summary>
        /// <param name="filterContext">Filter context</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session != null)
            {
                ViewData[Constants.ViewData.Notify] = Session[Constants.ViewData.Notify] as Notify ?? new Notify();    
            }

            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        ///     Set notify
        /// </summary>
        /// <param name="notify">
        ///     <see>
        ///         <cref>Notify</cref>
        ///     </see>
        /// </param>
        public void SetNotify(Notify notify)
        {
            Session[Constants.ViewData.Notify] = notify ?? new Notify();
        }

        /// <summary>
        ///     Notify result
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>
        ///     <see cref="NotifyResult" />
        /// </returns>
        public NotifyResult Notify(string message)
        {
            return Notify(new List<string> {message});
        }

        /// <summary>
        ///     Notify result
        /// </summary>
        /// <param name="messages">Message</param>
        /// <returns>
        ///     <see cref="NotifyResult" />
        /// </returns>
        public NotifyResult Notify(IList<string> messages)
        {
            return Notify(NotifyType.Success, messages);
        }

        /// <summary>
        ///     Notify result
        /// </summary>
        /// <param name="notifyType">
        ///     <see cref="NotifyType" />
        /// </param>
        /// <param name="message">Message</param>
        /// <returns>
        ///     <see cref="NotifyResult" />
        /// </returns>
        public NotifyResult Notify(NotifyType notifyType, string message)
        {
            return Notify(notifyType, new List<string> {message});
        }

        /// <summary>
        ///     Notify result
        /// </summary>
        /// <param name="notifyType">
        ///     <see cref="NotifyType" />
        /// </param>
        /// <param name="messages">Messages</param>
        /// <returns>
        ///     <see cref="NotifyResult" />
        /// </returns>
        public NotifyResult Notify(NotifyType notifyType, IList<string> messages)
        {
            return new NotifyResult(notifyType, messages);
        }

        /// <summary>
        ///     Notify result
        /// </summary>
        /// <param name="notifyType">
        ///     <see cref="NotifyType" />
        /// </param>
        /// <param name="modelStates">
        ///     <see cref="IDictionary{TKey,TValue}" />
        /// </param>
        /// <returns>
        ///     <see cref="NotifyResult" />
        /// </returns>
        public NotifyResult Notify(NotifyType notifyType, IDictionary<string, ModelState> modelStates)
        {
            return new NotifyResult(notifyType, modelStates);
        }

        /// <summary>
        ///     Notify result
        /// </summary>
        /// <param name="notify">
        ///     <see>
        ///         <cref>Notify</cref>
        ///     </see>
        /// </param>
        /// <returns>
        ///     <see cref="NotifyResult" />
        /// </returns>
        public NotifyResult Notify(Notify notify)
        {
            return new NotifyResult(notify);
        }

        /// <summary>
        ///     Success json result
        /// </summary>
        /// <returns></returns>
        public JsonResult Success()
        {
            return Success(null);
        }

        /// <summary>
        ///     Success json result
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public JsonResult Success(object data)
        {
            return Success(data, "application/json");
        }

        /// <summary>
        ///     Success json result
        /// </summary>
        /// <param name="data"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public JsonResult Success(object data, string contentType)
        {
            return Success(data, "application/json", JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        ///     Success json result
        /// </summary>
        /// <param name="data"></param>
        /// <param name="jsonRequestBehavior"></param>
        /// <returns></returns>
        public JsonResult Success(object data, JsonRequestBehavior jsonRequestBehavior)
        {
            return Success(data, "application/json", jsonRequestBehavior);
        }

        /// <summary>
        ///     Success json result
        /// </summary>
        /// <param name="data"></param>
        /// <param name="contentType"></param>
        /// <param name="jsonRequestBehavior"></param>
        /// <returns></returns>
        public JsonResult Success(object data, string contentType, JsonRequestBehavior jsonRequestBehavior)
        {
            return new JsonResult
            {
                Type = JsonResultType.Success,
                Data = data,
                JsonRequestBehavior = jsonRequestBehavior,
                ContentType = contentType
            };
        }

        /// <summary>
        ///     Convert dictionary <see cref="ModelState" /> to list of key and value
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, IList<string>>> ToKeyAndValues(
            IDictionary<string, ModelState> modelState)
        {
            return modelState.ToKeyAndValues();
        }

        /// <summary>
        ///     Render partial view
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns>View</returns>
        protected string RenderPartial(object model)
        {
            return RenderPartial(null, model);
        }

        /// <summary>
        ///     Render partial view
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
        ///     Exclude properties in validation of model state
        /// </summary>
        /// <param name="properties">Properties</param>
        public void ExcludePropertiesInValidation(params string[] properties)
        {
            if (ModelState.IsValid) return;

            foreach (var property in properties.Where(property => ModelState.ContainsKey(property)))
            {
                ModelState.Remove(property);
            }
        }

        /// <summary>
        /// Exclude properties with key part in validation of model state
        /// </summary>
        /// <param name="keyPart">Key part in validation of model state</param>
        /// <param name="ignoreKeys">Ignore keys</param>
        public void ExcludePropertiesWithKeyPart(string keyPart, params string[] ignoreKeys)
        {
            ModelState.ExcludePropertiesWithKeyPart(keyPart, ignoreKeys);
        }

        /// <summary>
        ///     OnException
        /// </summary>
        /// <param name="filterContext">
        ///     <see cref="ExceptionContext" />
        /// </param>
        protected override void OnException(ExceptionContext filterContext)
        {
            Logging.Write.Error(filterContext.Exception);

            base.OnException(filterContext);
        }
    }
}