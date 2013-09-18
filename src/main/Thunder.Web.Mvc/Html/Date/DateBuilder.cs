using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Thunder.Web.Mvc.Html.Date
{
    public class DateBuilder<TModel>
    {
        private readonly HtmlHelper<TModel> _helper;

        public DateBuilder(HtmlHelper<TModel> helper)
        {
            _helper = helper;
        }

        public MvcHtmlString Builder<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            var attributes = HtmlAttributesUtility.ObjectToHtmlAttributesDictionary(htmlAttributes);

            attributes.AddMaxLengthAttribute(expression, 10);
            attributes.AddCssClass("date");

            return _helper.TextBoxFor(expression, attributes);
        }


    }
}
