using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using Thunder.Collections;

namespace Thunder.Web.Mvc.Html.Pagination
{
    /// <summary>
    /// Pagination html builder
    /// </summary>
    public class PaginationBuilder
    {
        /// <summary>
        /// Builder
        /// </summary>
        /// <param name="source"></param>
        /// <param name="url"></param>
        /// <param name="size"></param>
        /// <param name="htmlAttributes"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public MvcHtmlString Builder<T>(IPaging<T> source, Func<int, string> url,  int size, object htmlAttributes)
        {
            var attributes = HtmlAttributesUtility.ObjectToHtmlAttributesDictionary(htmlAttributes);
            var ul = new TagBuilder("ul");

            attributes.AddCssClass("pagination");

            ul.MergeAttributes(attributes);
            ul.InnerHtml += Item(url, source.HasPreviousPage ? (source.CurrentPage - 1) : -1, "&laquo;", false, !source.HasPreviousPage);

            foreach (var page in Pages(source, size))
            {
                ul.InnerHtml += Item(url, page, (page + 1).ToString(CultureInfo.InvariantCulture), page == source.CurrentPage);    
            }

            ul.InnerHtml += Item(url, source.HasNextPage ? (source.CurrentPage + 1) : -1, "&raquo;", false, !source.HasNextPage);

            return MvcHtmlString.Create(ul.ToString());
        }

        private static string Item(Func<int, string> url, int page, string label, bool active = false, bool disabled = false)
        {
            var li = new TagBuilder("li");
            var link = new TagBuilder("a");

            if (active)
            {
                li.AddCssClass("active");
            }

            if (disabled)
            {
                li.AddCssClass("disabled");
            }

            link.Attributes.Add("data-page", page.ToString(CultureInfo.InvariantCulture));
            link.Attributes.Add("href", (url == null || page < 0 ? "#" : url(page)));
            
            link.InnerHtml = label;

            li.InnerHtml = link.ToString();

            return li.ToString();
        }

        private static IEnumerable<int> Pages<T>(IPaging<T> source, int size)
        {
            var pages = new List<int>();
            var start = 0;
            var end = source.PageCount;

            size--;

            if (source.PageCount >= size)
            {
                start = 0;
                end = size;

                if (source.CurrentPage > Convert.ToInt32(size / 2))
                {
                    if (source.CurrentPage > (source.PageCount - Convert.ToInt32(size / 2)))
                    {
                        start = source.PageCount - size;
                        end = source.PageCount;
                    }
                    else
                    {
                        start = source.CurrentPage - Convert.ToInt32(size / 2);
                        end = source.CurrentPage + Convert.ToInt32(size / 2);
                    }
                }
            }

            if (end == source.PageCount)
            {
                start--;
                end--;
            }

            for (var i = start; i <= end; i++)
            {
                pages.Add(i);
            }

            return pages;
        }
    }
}
