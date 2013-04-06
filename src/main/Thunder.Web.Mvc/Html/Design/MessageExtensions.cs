using System.Collections.Generic;
using System.Web.Mvc;

namespace Thunder.Web.Mvc.Html.Design
{
    /// <summary>
    /// Message extensions
    /// </summary>
    public static class MessageExtensions
    {
        /// <summary>
        /// Message success
        /// </summary>
        /// <param name="htmlHelper"><see cref="HtmlHelper"/></param>
        /// <param name="message">Message</param>
        /// <param name="showCloseButton">Show close button</param>
        /// <returns><see cref="MvcHtmlString"/></returns>
        public static MvcHtmlString Success(this HtmlHelper htmlHelper, string message, bool showCloseButton = false)
        {
            return htmlHelper.Success(new List<string> {message}, showCloseButton);
        }

        /// <summary>
        /// Messages success
        /// </summary>
        /// <param name="htmlHelper"><see cref="HtmlHelper"/></param>
        /// <param name="messages">Collections of message</param>
        /// <param name="showCloseButton">Show close button</param>
        /// <returns><see cref="MvcHtmlString"/></returns>
        public static MvcHtmlString Success(this HtmlHelper htmlHelper, IList<string> messages, bool showCloseButton = false)
        {
            return Message("thunder-success", messages, showCloseButton);
        }

        /// <summary>
        /// Message attention
        /// </summary>
        /// <param name="htmlHelper"><see cref="HtmlHelper"/></param>
        /// <param name="message">Message</param>
        /// <param name="showCloseButton">Show close button</param>
        /// <returns><see cref="MvcHtmlString"/></returns>
        public static MvcHtmlString Attention(this HtmlHelper htmlHelper, string message, bool showCloseButton = false)
        {
            return htmlHelper.Attention(new List<string> {message}, showCloseButton);
        }

        /// <summary>
        /// Messages attention
        /// </summary>
        /// <param name="htmlHelper"><see cref="HtmlHelper"/></param>
        /// <param name="messages">Collections of message</param>
        /// <param name="showCloseButton">Show close button</param>
        /// <returns><see cref="MvcHtmlString"/></returns>
        public static MvcHtmlString Attention(this HtmlHelper htmlHelper, IList<string> messages, bool showCloseButton = false)
        {
            return Message("thunder-attention", messages, showCloseButton);
        }

        /// <summary>
        /// Message information
        /// </summary>
        /// <param name="htmlHelper"><see cref="HtmlHelper"/></param>
        /// <param name="message">Message</param>
        /// <param name="showCloseButton">Show close button</param>
        /// <returns><see cref="MvcHtmlString"/></returns>
        public static MvcHtmlString Information(this HtmlHelper htmlHelper, string message, bool showCloseButton = false)
        {
            return htmlHelper.Information(new List<string> {message}, showCloseButton);
        }

        /// <summary>
        /// Messages information
        /// </summary>
        /// <param name="htmlHelper"><see cref="HtmlHelper"/></param>
        /// <param name="messages">Collections of message</param>
        /// <param name="showCloseButton">Show close button</param>
        /// <returns><see cref="MvcHtmlString"/></returns>
        public static MvcHtmlString Information(this HtmlHelper htmlHelper, IList<string> messages, bool showCloseButton = false)
        {
            return Message("thunder-information", messages, showCloseButton);
        }

        /// <summary>
        /// Message error
        /// </summary>
        /// <param name="htmlHelper"><see cref="HtmlHelper"/></param>
        /// <param name="message">Message</param>
        /// <param name="showCloseButton">Show close button</param>
        /// <returns><see cref="MvcHtmlString"/></returns>
        public static MvcHtmlString Error(this HtmlHelper htmlHelper, string message, bool showCloseButton = false)
        {
            return htmlHelper.Error(new List<string> {message}, showCloseButton);
        }

        /// <summary>
        /// Messages error
        /// </summary>
        /// <param name="htmlHelper"><see cref="HtmlHelper"/></param>
        /// <param name="messages">Collections of message</param>
        /// <param name="showCloseButton">Show close button</param>
        /// <returns><see cref="MvcHtmlString"/></returns>
        public static MvcHtmlString Error(this HtmlHelper htmlHelper, IList<string> messages, bool showCloseButton = false)
        {
            return Message("thunder-error", messages, showCloseButton);
        }

        private static MvcHtmlString Message(string cssClass, IList<string> messages, bool showCloseButton = false)
        {
            if (messages.Count.Equals(0))
            {
                return MvcHtmlString.Empty;
            }

            var notification = new TagBuilder("div");
            var content = new TagBuilder("div");
            var close = new TagBuilder("a");

            notification.AddCssClass("thunder-notification");
            notification.AddCssClass(cssClass);

            content.AddCssClass("thunder-notification-content");

            close.AddCssClass("close");
            close.Attributes.Add("href", "#");

            if (messages.Count.Equals(1))
            {
                content.InnerHtml = messages[0];
            }
            else
            {
                var ul = new TagBuilder("ul");
                foreach (var message in messages)
                {
                    ul.InnerHtml += new TagBuilder("li") {InnerHtml = message}.ToString();
                }
                content.InnerHtml = ul.ToString();
            }

            notification.InnerHtml += content.ToString();

            if (showCloseButton)
            {
                notification.InnerHtml += close.ToString();                
            }

            return MvcHtmlString.Create(notification.ToString());
        }
    }
}