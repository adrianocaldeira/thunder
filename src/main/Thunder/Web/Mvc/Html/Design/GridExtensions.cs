using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Routing;

namespace Thunder.Web.Mvc.Html.Design
{
    /// <summary>
    /// Grid extensions
    /// </summary>
    public static class GridExtensions
    {
        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="htmlHelper"><see cref="HtmlHelper"/></param>
        /// <param name="url">Url</param>
        /// <returns><see cref="MvcGrid"/></returns>
        public static MvcGrid Grid(this HtmlHelper htmlHelper, string url)
        {
            return htmlHelper.GridHelper(url, 0, null);
        }

        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="htmlHelper"><see cref="HtmlHelper"/></param>
        /// <param name="url">Url</param>
        /// <param name="htmlAttributes">Html Attributes</param>
        /// <returns><see cref="MvcGrid"/></returns>
        public static MvcGrid Grid(this HtmlHelper htmlHelper, string url, object htmlAttributes)
        {
            return htmlHelper.GridHelper(url, 0, htmlAttributes);
        }

        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="htmlHelper"><see cref="HtmlHelper"/></param>
        /// <param name="url">Url</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="htmlAttributes">Html Attributes</param>
        /// <returns><see cref="MvcGrid"/></returns>
        public static MvcGrid Grid(this HtmlHelper htmlHelper, string url, int pageSize, object htmlAttributes)
        {
            return htmlHelper.GridHelper(url, pageSize, htmlAttributes);
        }

        private static MvcGrid GridHelper(this HtmlHelper htmlHelper, string url, int pageSize, object htmlAttributes)
        {
            var grid = new TagBuilder("div");
            var textWriter = htmlHelper.ViewContext.Writer;

            if (htmlAttributes != null)
            {
                if (htmlAttributes is Dictionary<string, object>)
                {
                    grid.MergeAttributes((Dictionary<string, object>)htmlAttributes);
                }
                else
                {
                    grid.MergeAttributes(new RouteValueDictionary(htmlAttributes));
                }
            }
            else
            {
                grid.MergeAttributes(new RouteValueDictionary());
            }

            if (grid.Attributes.ContainsKey("class"))
            {
                grid.Attributes["class"] = "grid " + grid.Attributes["class"];
            }
            else
            {
                grid.AddCssClass("grid");
            }

            if(!string.IsNullOrEmpty(url))
            {
                grid.Attributes.Add("data-url", url);
            }

            if (pageSize == 0)
                pageSize = 15;

            grid.Attributes.Add("data-page-size", pageSize.ToString(CultureInfo.InvariantCulture));

            textWriter.Write(grid.ToString(TagRenderMode.StartTag));

            return new MvcGrid(htmlHelper.ViewContext);
        }
    }
}
