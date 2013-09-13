using System.Web.Mvc;

namespace Thunder.Web.Mvc.Html.Grid
{
    public class GridBuilder<TModel>
    {
        private readonly HtmlHelper<TModel> _helper;

        public GridBuilder(HtmlHelper<TModel> helper)
        {
            _helper = helper;
        }

        public MvcGrid Builder(string url, int pageSize, object htmlAttributes)
        {
            var tagBuilder = new TagBuilder("div");
            var textWriter = _helper.ViewContext.Writer;
            var attributes = HtmlAttributesUtility.ObjectToHtmlAttributesDictionary(htmlAttributes);

            if (pageSize == 0)
                pageSize = 15;

            attributes.AddCssClass("thunder-grid");
            attributes.MergeAttribute("data-url", url);
            attributes.MergeAttribute("data-page-size", pageSize);

            tagBuilder.MergeAttributes(attributes);

            textWriter.Write(tagBuilder.ToString(TagRenderMode.StartTag));

            return new MvcGrid(_helper.ViewContext);
        }
    }
}
