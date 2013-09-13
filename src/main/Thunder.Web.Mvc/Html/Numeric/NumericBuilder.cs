using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Thunder.Web.Mvc.Html.Numeric
{
    public class NumericBuilder<TModel>
    {
        private readonly HtmlHelper<TModel> _helper;

        public NumericBuilder(HtmlHelper<TModel> helper)
        {
            _helper = helper;
        }

        public MvcHtmlString Builder<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            var attributes = HtmlAttributesUtility.ObjectToHtmlAttributesDictionary(htmlAttributes);

            attributes.AddMaxLengthAttribute(expression);
            attributes.AddCssClass("numeric");

            return _helper.TextBoxFor(expression, attributes);
        }
    }
}
