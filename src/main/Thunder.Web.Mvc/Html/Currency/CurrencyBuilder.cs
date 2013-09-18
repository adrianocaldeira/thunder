using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Thunder.Web.Mvc.Html.Currency
{
    public class CurrencyBuilder<TModel>
    {
        private readonly HtmlHelper<TModel> _helper;

        public CurrencyBuilder(HtmlHelper<TModel> helper)
        {
            _helper = helper;
        }

        public MvcHtmlString Builder<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            var attributes = HtmlAttributesUtility.ObjectToHtmlAttributesDictionary(htmlAttributes);

            attributes.AddMaxLengthAttribute(expression);
            attributes.AddCssClass("currency");

            return _helper.TextBoxFor(expression, attributes);
        }
    }
}
