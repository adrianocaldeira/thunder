using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Thunder.Web.Mvc.Html.ZipCode
{
    public class ZipCodeBuilder<TModel>
    {
        private readonly HtmlHelper<TModel> _helper;

        public ZipCodeBuilder(HtmlHelper<TModel> helper)
        {
            _helper = helper;
        }

        public MvcHtmlString Builder<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            var attributes = HtmlAttributesUtility.ObjectToHtmlAttributesDictionary(htmlAttributes);

            attributes.AddMaxLengthAttribute(expression, 9);
            attributes.AddCssClass("zip-code");

            return _helper.TextBoxFor(expression, attributes);
        }


    }
}
