using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Thunder.Web.Mvc.Html.TextArea
{
    public class TextAreaBuilder<TModel>
    {
        private readonly HtmlHelper<TModel> _helper;

        public TextAreaBuilder(HtmlHelper<TModel> helper)
        {
            _helper = helper;
        }

        public MvcHtmlString Builder<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            var attributes = HtmlAttributesUtility.ObjectToHtmlAttributesDictionary(htmlAttributes);

            attributes.AddMaxLengthAttribute(expression);

            return _helper.TextAreaFor(expression, attributes);
        }
    }
}
