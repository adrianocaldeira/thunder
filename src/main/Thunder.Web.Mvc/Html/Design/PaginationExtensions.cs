using System.Globalization;
using System.Web.Mvc;
using Thunder.Collections;

namespace Thunder.Web.Mvc.Html.Design
{
    /// <summary>
    /// Pagination extensions
    /// </summary>
    public static class PaginationExtension
    {
        /// <summary>
        /// Pagination
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <param name="data">Data</param>
        /// <typeparam name="T">Type</typeparam>
        /// <returns><see cref="MvcHtmlString"/></returns>
        public static MvcHtmlString Pagination<T>(this HtmlHelper helper, IPaging<T> data)
        {
            var pagination = new TagBuilder("div");
            var paginationLoading = new TagBuilder("div");
            var paginationPrevious = new TagBuilder("div");
            var paginationNext = new TagBuilder("div");
            var paginationPages = new TagBuilder("div");
            var loading = new TagBuilder("span");
            var previous = new TagBuilder("a");
            var next = new TagBuilder("a");
            var pages = new TagBuilder("select");

            pagination.AddCssClass("thunder-pagination");

            paginationLoading.AddCssClass("thunder-pagination-loading");
            paginationPrevious.AddCssClass("thunder-pagination-previous");
            paginationNext.AddCssClass("thunder-pagination-next");
            paginationPages.AddCssClass("thunder-pagination-pages");

            loading.AddCssClass("thunder-paged-loading");
            loading.InnerHtml = "Carregando";
            loading.MergeAttribute("style","display: none;");
            
            previous.MergeAttribute("href", "#");
            previous.MergeAttribute("title", "Anterior");
            previous.InnerHtml = "&laquo;";
            previous.AddCssClass("thunder-grid-paged" + (!data.HasPreviousPage ? " disabled" : ""));

            next.MergeAttribute("href", "#");
            next.MergeAttribute("title", "Próximo");
            next.InnerHtml = "&raquo;";
            next.AddCssClass("thunder-grid-paged" + (!data.HasNextPage ? " disabled" : ""));

            pages.AddCssClass("thunder-grid-paged");

            for (var i = 0; i < data.PageCount; i++)
            {
                var option = new TagBuilder("option");

                option.MergeAttribute("value", i.ToString(CultureInfo.InvariantCulture));

                if (i == data.CurrentPage)
                {
                    option.MergeAttribute("selected", "selected");
                }

                option.InnerHtml = string.Format("P&aacute;gina {0} de {1}", (i + 1), data.PageCount);
                pages.InnerHtml += option;
            }

            if (data.HasPreviousPage)
            {
                previous.MergeAttribute("data-page", (data.CurrentPage - 1).ToString(CultureInfo.InvariantCulture));
            }

            if (data.HasNextPage)
            {
                next.MergeAttribute("data-page", (data.CurrentPage + 1).ToString(CultureInfo.InvariantCulture));
            }

            paginationLoading.InnerHtml = loading.ToString();
            paginationPrevious.InnerHtml = previous.ToString();
            paginationNext.InnerHtml = next.ToString();
            paginationPages.InnerHtml = pages.ToString();

            pagination.InnerHtml += paginationLoading.ToString();
            pagination.InnerHtml += paginationPrevious.ToString();
            pagination.InnerHtml += paginationPages.ToString();
            pagination.InnerHtml += paginationNext.ToString();

            return MvcHtmlString.Create(pagination.ToString());
        }
    }
}
