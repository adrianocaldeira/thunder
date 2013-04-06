using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace Thunder.Web.Mvc.Html.Design.SimplaAdmin
{
    /// <summary>
    /// Content box extensions
    /// </summary>
    public static class ContentBoxExtensions
    {
        /// <summary>
        /// Content box
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <param name="title">Title</param>
        /// <param name="loading">Loading</param>
        /// <returns><see cref="MvcContentBox"/></returns>
        public static MvcContentBox ContentBox(this HtmlHelper htmlHelper, string title, ContentBoxHeaderLoading loading)
        {
            return htmlHelper.ContentBoxHelper(new ContentBoxHeader { Title = title, Loading = loading}, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="title"></param>
        /// <param name="loading"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcContentBox ContentBox(this HtmlHelper htmlHelper, string title, ContentBoxHeaderLoading loading, object htmlAttributes)
        {
            return htmlHelper.ContentBoxHelper(new ContentBoxHeader { Title = title, Loading = loading }, htmlAttributes);
        }

        /// <summary>
        /// Content box
        /// </summary>
        /// <param name="htmlHelper">Html helper</param>
        /// <param name="title">Title</param>
        /// <returns>MvcContentBox</returns>
        public static MvcContentBox ContentBox(this HtmlHelper htmlHelper, string title)
        {
            return htmlHelper.ContentBoxHelper(new ContentBoxHeader { Title = title }, null);
        }

        /// <summary>
        /// Content box
        /// </summary>
        /// <param name="htmlHelper">Html helper</param>
        /// <param name="title">Title</param>
        /// <param name="htmlAttributes">Html Attributes</param>
        /// <returns>MvcContentBox</returns>
        public static MvcContentBox ContentBox(this HtmlHelper htmlHelper, string title, object htmlAttributes)
        {
            return htmlHelper.ContentBoxHelper(new ContentBoxHeader { Title = title }, htmlAttributes);
        }

        /// <summary>
        /// Content box
        /// </summary>
        /// <param name="htmlHelper">Html helper</param>
        /// <param name="title">Title</param>
        /// <param name="required">Required</param>
        /// <returns>MvcContentBox</returns>
        public static MvcContentBox ContentBox(this HtmlHelper htmlHelper, string title, bool  required)
        {
            return htmlHelper.ContentBoxHelper(new ContentBoxHeader { Title = title, Required = required }, null);
        }

        /// <summary>
        /// Content box
        /// </summary>
        /// <param name="htmlHelper">Html helper</param>
        /// <param name="title">Title</param>
        /// <param name="required">Required</param>
        /// <param name="htmlAttributes">Html Attributes</param>
        /// <returns>MvcContentBox</returns>
        public static MvcContentBox ContentBox(this HtmlHelper htmlHelper, string title, bool required, object htmlAttributes)
        {
            return htmlHelper.ContentBoxHelper(new ContentBoxHeader { Title = title, Required = required }, htmlAttributes);
        }

        /// <summary>
        /// Content box
        /// </summary>
        /// <param name="htmlHelper">Html helper</param>
        /// <param name="title">Title</param>
        /// <param name="buttons">Buttons</param>
        /// <returns>MvcContentBox</returns>
        public static MvcContentBox ContentBox(this HtmlHelper htmlHelper, string title, params ContentBoxHeaderButton[] buttons)
        {
            return htmlHelper.ContentBoxHelper(new ContentBoxHeader { Title = title, Buttons = buttons.ToList()}, null);
        }

        /// <summary>
        /// Content box
        /// </summary>
        /// <param name="htmlHelper">Html helper</param>
        /// <param name="title">Title</param>
        /// <param name="required">Required</param>
        /// <param name="buttons">Buttons</param>
        /// <returns>MvcContentBox</returns>
        public static MvcContentBox ContentBox(this HtmlHelper htmlHelper, string title, bool required, params ContentBoxHeaderButton[] buttons)
        {
            return htmlHelper.ContentBoxHelper(new ContentBoxHeader { Title = title, Required = required, Buttons = buttons.ToList() }, null);
        }

        /// <summary>
        /// Content box
        /// </summary>
        /// <param name="htmlHelper">Html helper</param>
        /// <param name="title">Title</param>
        /// <param name="tabs">Tabs</param>
        /// <returns>MvcContentBox</returns>
        public static MvcContentBox ContentBox(this HtmlHelper htmlHelper, string title, params ContentBoxHeaderTab[] tabs)
        {
            return htmlHelper.ContentBox(title, false, tabs);
        }

        /// <summary>
        /// Content box
        /// </summary>
        /// <param name="htmlHelper">Html helper</param>
        /// <param name="title">Title</param>
        /// <param name="required">Required</param>
        /// <param name="tabs">Tabs</param>
        /// <returns>MvcContentBox</returns>
        public static MvcContentBox ContentBox(this HtmlHelper htmlHelper, string title, bool required, params ContentBoxHeaderTab[] tabs)
        {
            return htmlHelper.ContentBox(title, required, null, tabs);
        }

        /// <summary>
        /// Content box
        /// </summary>
        /// <param name="htmlHelper">Html helper</param>
        /// <param name="title">Title</param>
        /// <param name="required">Required</param>
        /// <param name="htmlAttributes">Html attributes</param>
        /// <param name="tabs">Tabs</param>
        /// <returns>MvcContentBox</returns>
        public static MvcContentBox ContentBox(this HtmlHelper htmlHelper, string title, bool required, object htmlAttributes, params ContentBoxHeaderTab[] tabs)
        {
            return htmlHelper.ContentBoxHelper(new ContentBoxHeader
            {
                Title = title,
                Required = required,
                Tabs = tabs.ToList()
            }, htmlAttributes);
        }

        /// <summary>
        /// Content box helper
        /// </summary>
        /// <param name="htmlHelper">Html helper</param>
        /// <param name="header">Header</param>
        /// <param name="htmlAttributes">Html attributes</param>
        /// <returns>MvcContentBox</returns>
        private static MvcContentBox ContentBoxHelper(this HtmlHelper htmlHelper, ContentBoxHeader header, object htmlAttributes)
        {
            var textWriter = htmlHelper.ViewContext.Writer;
            var contentBox = new TagBuilder("div");

            if (htmlAttributes != null)
            {
                if (htmlAttributes is Dictionary<string, object>)
                {
                    contentBox.MergeAttributes((Dictionary<string, object>)htmlAttributes);
                }
                else
                {
                    contentBox.MergeAttributes(new RouteValueDictionary(htmlAttributes));
                }
            }
            else
            {
                contentBox.MergeAttributes(new RouteValueDictionary());
            }

            if(contentBox.Attributes.ContainsKey("class"))
            {
                contentBox.Attributes["class"] = "content-box " + contentBox.Attributes["class"];
            }
            else
            {
                contentBox.AddCssClass("content-box");
            }

            textWriter.Write(contentBox.ToString(TagRenderMode.StartTag));
            textWriter.Write(Header(htmlHelper, header));
            textWriter.Write(Content());

            return new MvcContentBox(htmlHelper.ViewContext);
        }

        /// <summary>
        /// Header
        /// </summary>
        /// <param name="htmlHelper">Html header</param>
        /// <param name="header">header</param>
        /// <returns>Hearder</returns>
        private static string Header(HtmlHelper htmlHelper, ContentBoxHeader header)
        {
            var builder = new TagBuilder("div");
            var h3 = new TagBuilder("h3");
            var clear = new TagBuilder("div");

            clear.AddCssClass("clear");

            h3.SetInnerText(header.Title);

            if (header.Required)
            {
                var span = new TagBuilder("span");
                span.AddCssClass("required");
                span.SetInnerText("*");

                h3.InnerHtml += "&nbsp;";
                h3.InnerHtml += span;
            }

            builder.AddCssClass("content-box-header");
            builder.InnerHtml += h3.ToString();

            CreateHeaderLoading(header, builder, htmlHelper);
            CreateHeaderTabs(header, builder);
            CreateHeaderButtons(header, builder, htmlHelper);

            builder.InnerHtml += clear.ToString();

            return builder.ToString();
        }

        /// <summary>
        /// Create header buttons
        /// </summary>
        /// <param name="header">Header</param>
        /// <param name="builder">Builder</param>
        /// <param name="htmlHelper">Html helper</param>
        private static void CreateHeaderButtons(ContentBoxHeader header, TagBuilder builder, HtmlHelper htmlHelper)
        {
            if (header.Buttons == null || header.Buttons.Count.Equals(0)) return;

            var ul = new TagBuilder("ul");

            ul.AddCssClass("content-box-tabs content-box-tabs-button");

            foreach (var button in header.Buttons)
            {
                var li = new TagBuilder("li");
                var link = new TagBuilder("a");
                var url = UrlHelper.GenerateContentUrl(button.Url, htmlHelper.ViewContext.HttpContext);

                if (button.HtmlAttributes != null)
                {
                    if(button.HtmlAttributes is Dictionary<string, object>)
                    {
                        link.MergeAttributes((Dictionary<string, object>)button.HtmlAttributes);
                    }
                    else
                    {
                        link.MergeAttributes(new RouteValueDictionary(button.HtmlAttributes));
                    }
                }
                else
                {
                    link.MergeAttributes(new RouteValueDictionary());
                }

                link.MergeAttribute("class", button.Class);
                link.MergeAttribute("href", url, true);
                link.MergeAttribute("title", button.Title, true);
                link.InnerHtml = button.Text;

                li.InnerHtml = link.ToString();
                ul.InnerHtml += li.ToString();
            }

            builder.InnerHtml += ul.ToString();
        }

        /// <summary>
        /// Create header tabs
        /// </summary>
        /// <param name="header">Header</param>
        /// <param name="builder">Builder</param>
        private static void CreateHeaderTabs(ContentBoxHeader header, TagBuilder builder)
        {
            if (header.Tabs == null || header.Tabs.Count <= 0) return;

            var ul = new TagBuilder("ul");
            int i = 0;

            ul.AddCssClass("content-box-tabs");

            foreach (ContentBoxHeaderTab tab in header.Tabs)
            {
                var li = new TagBuilder("li");
                var link = new TagBuilder("a");

                if (tab.Current)
                {
                    link.AddCssClass("default-tab current");
                }

                link.Attributes.Add("href", "#tab" + i);
                link.Attributes.Add("title", tab.Title);
                link.InnerHtml = tab.Text;

                li.InnerHtml = link.ToString();
                ul.InnerHtml += li.ToString();

                i++;
            }

            builder.InnerHtml += ul.ToString();
        }

        /// <summary>
        /// Create header loading
        /// </summary>
        /// <param name="header">Header</param>
        /// <param name="builder">Builder</param>
        /// <param name="htmlHelper">Html helper</param>
        private static void CreateHeaderLoading(ContentBoxHeader header, TagBuilder builder, HtmlHelper htmlHelper)
        {
            if (header.Loading == null) return;

            var loading = new TagBuilder("div");
            string url = UrlHelper.GenerateContentUrl(header.Loading.ImagePath, htmlHelper.ViewContext.HttpContext);

            loading.AddCssClass(header.Loading.Class);
            loading.Attributes.Add("id", header.Loading.Id);
            loading.InnerHtml = "<img src=\"" + url + "\" alt=\"\" />";

            builder.InnerHtml += loading.ToString();
        }

        /// <summary>
        /// Content
        /// </summary>
        /// <returns>Content</returns>
        private static string Content()
        {
            var content = new TagBuilder("div");

            content.AddCssClass("content-box-content");

            return content.ToString(TagRenderMode.StartTag);
        }
    }
}