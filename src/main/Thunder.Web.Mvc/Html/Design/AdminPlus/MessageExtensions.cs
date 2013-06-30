using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Thunder.Web.Mvc.Html.Design.AdminPlus
{
    /// <summary>
    /// Message extensions
    /// </summary>
    public static class MessageExtensions
    {
        /// <summary>
        /// Message
        /// </summary>
        /// <param name="helper"><see cref="HtmlHelper"/></param>
        /// <param name="type"><see cref="MessageType"/></param>
        /// <param name="message">Message</param>
        /// <returns><see cref="MvcHtmlString"/></returns>
        public static MvcHtmlString Message(this HtmlHelper helper, MessageType type, string message)
        {
            return helper.Message(type, message, true);
        }

        /// <summary>
        /// Message
        /// </summary>
        /// <param name="helper"><see cref="HtmlHelper"/></param>
        /// <param name="type"><see cref="MessageType"/></param>
        /// <param name="messages"><see cref="IList{T}"/></param>
        /// <returns><see cref="MvcHtmlString"/></returns>
        public static MvcHtmlString Message(this HtmlHelper helper, MessageType type, IList<string> messages)
        {
            return helper.Message(type, messages, true);
        }

        /// <summary>
        /// Message
        /// </summary>
        /// <param name="helper"><see cref="HtmlHelper"/></param>
        /// <param name="type"><see cref="MessageType"/></param>
        /// <param name="message">Message</param>
        /// <param name="showCloseButton">Show close button</param>
        /// <returns><see cref="MvcHtmlString"/></returns>
        public static MvcHtmlString Message(this HtmlHelper helper, MessageType type, string message, bool showCloseButton)
        {
            return helper.Message(type, new List<string> { message }, showCloseButton);
        }

        /// <summary>
        /// Message
        /// </summary>
        /// <param name="helper"><see cref="HtmlHelper"/></param>
        /// <param name="type"><see cref="MessageType"/></param>
        /// <param name="messages"><see cref="IList{T}"/></param>
        /// <param name="showCloseButton">Show close button</param>
        /// <returns><see cref="MvcHtmlString"/></returns>
        public static MvcHtmlString Message(this HtmlHelper helper, MessageType type, IList<string> messages, bool showCloseButton)
        {
            var alert = new TagBuilder("div");
            var button = new TagBuilder("button");

            if (!messages.Any())
            {
                messages.Add("Nenhuma mensagem informada!");
            }

            alert.AddCssClass(GetCssClassByType(type));

            if (showCloseButton)
            {
                button.AddCssClass("close");
                button.Attributes.Add("data-dismiss", "alert");
                button.Attributes.Add("type", "button");
                button.InnerHtml = "×";
                alert.InnerHtml = button.ToString();
            }

            if (messages.Count > 1)
            {
                var list = new TagBuilder("ul");

                foreach (var message in messages)
                {
                    list.InnerHtml += (new TagBuilder("li") { InnerHtml = message }).ToString();
                }

                alert.InnerHtml += list.ToString();
            }
            else
            {
                alert.InnerHtml += messages.First();
            }

            return MvcHtmlString.Create(alert.ToString());
        }

        /// <summary>
        /// Get css class by type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string GetCssClassByType(MessageType type)
        {
            var cssClass = "alert";

            switch (type)
            {
                case MessageType.Information:
                    cssClass = string.Concat(cssClass, " ", "alert-info");
                    break;
                case MessageType.Error:
                    cssClass = string.Concat(cssClass, " ", "alert-error");
                    break;
                case MessageType.Success:
                    cssClass = string.Concat(cssClass, " ", "alert-success");
                    break;
            }

            return cssClass;
        }
    }
}
