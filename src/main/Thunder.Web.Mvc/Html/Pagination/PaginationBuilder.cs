using System;
using System.Linq;
using System.Web.Mvc;
using Thunder.Collections;
using Thunder.Extensions;

namespace Thunder.Web.Mvc.Html.Pagination
{
    public class PaginationBuilder
    {
        public MvcHtmlString Builder<T>(IPaging<T> source, int size, object htmlAttributes)
        {
            var attributes = HtmlAttributesUtility.ObjectToHtmlAttributesDictionary(htmlAttributes);
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
            
            clear.AddCssClass("clearfix");

            container.AddCssClass("pagination pull-right");
            container.Attributes.Add("style", "margin: 0;");
            container.InnerHtml += ul.ToString();

            pagination.MergeAttributes(attributes);
            pagination.AddCssClass("separator top form-inline small");
            pagination.InnerHtml += container.ToString();
            pagination.InnerHtml += clear.ToString();
            
            return MvcHtmlString.Create(pagination.ToString());
        }

        private int Skip<T>(int size, IPaging<T> source)
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
