using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Thunder.Web.Mvc.Html.Phone
{
    public class PhoneBuilder<TModel>
    {
        private readonly HtmlHelper<TModel> _helper;

        public PhoneBuilder(HtmlHelper<TModel> helper)
        {
            _helper = helper;
        }

        public MvcHtmlString Builder<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            var attributes = HtmlAttributesUtility.ObjectToHtmlAttributesDictionary(htmlAttributes);

            attributes.AddMaxLengthAttribute(expression, 15);
            attributes.AddCssClass("phone");

            return _helper.TextBoxFor(expression, attributes);
        }
    }
}
