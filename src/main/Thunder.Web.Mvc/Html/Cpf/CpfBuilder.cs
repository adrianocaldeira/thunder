using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Thunder.Web.Mvc.Html.Cpf
{
    public class CpfBuilder<TModel>
    {
        private readonly HtmlHelper<TModel> _helper;

        public CpfBuilder(HtmlHelper<TModel> helper)
        {
            _helper = helper;
        }

        public MvcHtmlString Builder<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            var attributes = HtmlAttributesUtility.ObjectToHtmlAttributesDictionary(htmlAttributes);

            attributes.AddMaxLengthAttribute(expression, 14);
            attributes.AddCssClass("cpf");

            return _helper.TextBoxFor(expression, attributes);
        }


    }
}
