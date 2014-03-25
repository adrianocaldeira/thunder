using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Thunder.Web.Mvc.Html.Cnpj
{
    /// <summary>
    /// Cnpj builder
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class CnpjBuilder<TModel>
    {
        private readonly HtmlHelper<TModel> _helper;

        /// <summary>
        /// Initialize new instance of <see cref="CnpjBuilder{TModel}"/>
        /// </summary>
        /// <param name="helper"></param>
        public CnpjBuilder(HtmlHelper<TModel> helper)
        {
            _helper = helper;
        }

        /// <summary>
        /// Builder
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <typeparam name="TProperty"></typeparam>
        /// <returns></returns>
        public MvcHtmlString Builder<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            var attributes = HtmlAttributesUtility.ObjectToHtmlAttributesDictionary(htmlAttributes);

            attributes.AddMaxLengthAttribute(expression, 18);
            attributes.AddCssClass("cnpj");

            return _helper.TextBoxFor(expression, attributes);
        }
    }
}
