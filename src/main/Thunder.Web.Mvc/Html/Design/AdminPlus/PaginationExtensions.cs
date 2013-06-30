using System;
using System.Linq;
using System.Web.Mvc;
using Thunder.Collections;
using Thunder.Extensions;

namespace Thunder.Web.Mvc.Html.Design.AdminPlus
{
    /// <summary>
    /// Paging extensions
    /// </summary>
    public static class PaginationExtensions
    {
        /// <summary>
        /// Pagination helper
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <param name="size">Size of pagination</param>
        /// <param name="source">Source</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static MvcHtmlString Pagination<T>(this HtmlHelper helper, int size, IPaging<T> source)
        {
            var pagination = new TagBuilder("div");
            var container = new TagBuilder("div");
            var clear = new TagBuilder("div");
            var ul = new TagBuilder("ul");
            var skip = Skip(size, source);

            if (source.HasPreviousPage)
            {
                ul.InnerHtml += "<li><a href=\"#\" class=\"thunder-grid-paged\" data-page=\"" + (source.CurrentPage - 1) + "\">«</a></li>";

            }
            else
            {
                ul.InnerHtml += "<li class=\"disabled\"><a href=\"#\" class=\"thunder-grid-paged disabled\">«</a></li>";
            }

            foreach (var page in source.PageCount.Times().Skip(skip).Take(size))
            {
                if (page == source.CurrentPage)
                {
                    ul.InnerHtml += "<li class=\"active\"><a href=\"#\" class=\"thunder-grid-paged disabled\">" +
                                    (page + 1) + "</a></li>";
                }
                else
                {
                    ul.InnerHtml += "<li><a href=\"#\" class=\"thunder-grid-paged\" data-page=\"" + page + "\">" + (page + 1) + "</a></li>";
                }
            }

            if (source.HasNextPage)
            {
                ul.InnerHtml += "<li><a href=\"#\" class=\"thunder-grid-paged\" data-page=\"" + (source.CurrentPage + 1) + "\">»</a></li>";
            }
            else
            {
                ul.InnerHtml += "<li class=\"disabled\"><a href=\"#\" class=\"thunder-grid-paged disabled\">»</a></li>";
            }

            pagination.AddCssClass("separator top form-inline small");
            container.AddCssClass("pagination pull-right");
            clear.AddCssClass("clearfix");

            container.Attributes.Add("style", "margin: 0;");

            container.InnerHtml += ul.ToString();
            pagination.InnerHtml += container.ToString();
            pagination.InnerHtml += clear.ToString();

            return MvcHtmlString.Create(pagination.ToString());
        }

        private static int Skip<T>(int size, IPaging<T> source)
        {
            var currentPage = (source.CurrentPage / size);
            var pageCount = (int)Math.Ceiling(source.PageCount / (double)size);
            var skip = (currentPage * size);

            if (pageCount <= currentPage)
            {
                skip = ((pageCount - 1) * size);
            }

            return skip;
        }
    }
}
