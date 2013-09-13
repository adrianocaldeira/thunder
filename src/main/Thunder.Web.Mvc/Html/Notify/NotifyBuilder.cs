using System.Linq;
using System.Web.Mvc;
using Thunder.Extensions;

namespace Thunder.Web.Mvc.Html.Notify
{
    public class NotifyBuilder
    {
        public MvcHtmlString Builder(Thunder.Notify notify, bool showCloseButton, object htmlAttributes)
        {
            var tagBuilder = new TagBuilder("div");
            var attributes = HtmlAttributesUtility.ObjectToHtmlAttributesDictionary(htmlAttributes);
            var content = string.Empty;

            if (showCloseButton)
            {
                var closeButton = new TagBuilder("button"){InnerHtml = "&times;"};
                
                closeButton.Attributes.Add("type","button");
                closeButton.Attributes.Add("class", "close");
                closeButton.Attributes.Add("data-dismiss", "alert");
                closeButton.Attributes.Add("aria-hidden", "true");

                content += closeButton.ToString();
            }

            if (notify.Messages.Count > 0)
            {
                if (notify.Messages.Count == 1)
                {
                    content += notify.Messages[0];
                }
                else
                {
                    var ul = new TagBuilder("ul");

                    foreach (var li in notify.Messages.Select(message => new TagBuilder("li") {InnerHtml = message}))
                    {
                        ul.InnerHtml += li.ToString();
                    }

                    content += ul.ToString();
                }
            }

            attributes.AddCssClass("alert{0}".With(CssClass(notify.Type)));
            tagBuilder.InnerHtml = content;

            tagBuilder.MergeAttributes(attributes);

            return MvcHtmlString.Create(tagBuilder.ToString());
        }

        private string CssClass(NotifyType type)
        {
            var cssClass = string.Empty;

            switch (type)
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
