using System.Linq;
using System.Web.Mvc;

namespace Thunder.Web.Mvc.Html.Design
{
    /// <summary>
    /// Grid sort extensions
    /// </summary>
    public static class GridSortExtensions
    {
        /// <summary>
        /// Sort
        /// </summary>
        /// <param name="helper"><see cref="HtmlHelper"/></param>
        /// <param name="text">Text</param>
        /// <param name="column">Column</param>
        /// <param name="filter"><see cref="Thunder.Model.Filter"/></param>
        /// <returns><see cref="MvcHtmlString"/></returns>
        public static MvcHtmlString GridSort(this HtmlHelper helper, string text, string column, Thunder.Model.Filter filter)
        {
            var a = new TagBuilder("a") { InnerHtml = text };
            var asc = true;

            foreach (var filterOrder in filter.Orders.Where(filterOrder => filterOrder.Column.ToLower().Equals(column.ToLower())))
            {
                asc = !filterOrder.Asc;
            }

            a.AddCssClass("thunder-grid-order");
            a.Attributes.Add("href", "#");
            a.Attributes.Add("data-column", column);
            a.Attributes.Add("data-asc", asc.ToString().ToLower());

            return MvcHtmlString.Create(a.ToString());
        }           
    }
}