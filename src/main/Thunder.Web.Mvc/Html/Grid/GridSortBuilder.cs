using System.Linq;
using System.Web.Mvc;

namespace Thunder.Web.Mvc.Html.Grid
{
    public class GridSortBuilder
    {
        public MvcHtmlString Builder(string text, string column, Thunder.Model.Filter filter, object htmlAttributes)
        {
            var tagBuilder = new TagBuilder("a") { InnerHtml = text };
            var attributes = HtmlAttributesUtility.ObjectToHtmlAttributesDictionary(htmlAttributes);
            var asc = true;

            foreach (var filterOrder in filter.Orders.Where(filterOrder => filterOrder.Column.ToLower().Equals(column.ToLower())))
            {
                asc = !filterOrder.Asc;
            }

            attributes.AddCssClass("thunder-grid-order");
            attributes.Add("href", "#");
            attributes.Add("data-column", column);
            attributes.Add("data-asc", asc.ToString().ToLower());

            tagBuilder.MergeAttributes(attributes);

            return MvcHtmlString.Create(tagBuilder.ToString());
        }
    }
}
