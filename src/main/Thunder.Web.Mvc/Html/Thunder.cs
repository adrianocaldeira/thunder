using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using Thunder.Web.Mvc.Html.Cnpj;
using Thunder.Web.Mvc.Html.Cpf;
using Thunder.Web.Mvc.Html.Currency;
using Thunder.Web.Mvc.Html.Date;
using Thunder.Web.Mvc.Html.Numeric;
using Thunder.Web.Mvc.Html.Password;
using Thunder.Web.Mvc.Html.Phone;
using Thunder.Web.Mvc.Html.TextArea;
using Thunder.Web.Mvc.Html.TextBox;
using Thunder.Web.Mvc.Html.ZipCode;

namespace Thunder.Web.Mvc.Html
{
    public class Thunder<TModel>
    {
        private readonly HtmlHelper<TModel> _helper;

        internal Thunder(HtmlHelper<TModel> helper)
        {
            _helper = helper;
        }

        #region TextBoxFor
        public MvcHtmlString TextBoxFor<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            return TextBoxFor(expression, null);
        }
        public MvcHtmlString TextBoxFor<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return new TextBoxBuilder<TModel>(_helper).Builder(expression, htmlAttributes);
        }
        #endregion

        #region TextAreaFor
        public MvcHtmlString TextAreaFor<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            return TextAreaFor(expression, null);
        }
        public MvcHtmlString TextAreaFor<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return new TextAreaBuilder<TModel>(_helper).Builder(expression, htmlAttributes);
        }
        #endregion

        #region PasswordFor
        public MvcHtmlString PasswordFor<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            return PasswordFor(expression, null);
        }
        public MvcHtmlString PasswordFor<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return new PasswordBuilder<TModel>(_helper).Builder(expression, htmlAttributes);
        }
        #endregion

        #region CnpjFor
        public MvcHtmlString CnpjFor<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            return CnpjFor(expression, null);
        }
        public MvcHtmlString CnpjFor<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return new CnpjBuilder<TModel>(_helper).Builder(expression, htmlAttributes);
        }
        #endregion

        #region CpfFor
        public MvcHtmlString CpfFor<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            return CpfFor(expression, null);
        }
        public MvcHtmlString CpfFor<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return new CpfBuilder<TModel>(_helper).Builder(expression, htmlAttributes);
        }
        #endregion

        #region CurrencyFor
        public MvcHtmlString CurrencyFor<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            return CurrencyFor(expression, null);
        }
        public MvcHtmlString CurrencyFor<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return new CurrencyBuilder<TModel>(_helper).Builder(expression, htmlAttributes);
        }
        #endregion

        #region DateFor
        public MvcHtmlString DateFor<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            return DateFor(expression, null);
        }
        public MvcHtmlString DateFor<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return new DateBuilder<TModel>(_helper).Builder(expression, htmlAttributes);
        }
        #endregion

        #region PhoneFor
        public MvcHtmlString PhoneFor<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            return PhoneFor(expression, null);
        }
        public MvcHtmlString PhoneFor<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return new PhoneBuilder<TModel>(_helper).Builder(expression, htmlAttributes);
        }
        #endregion

        #region NumericFor
        public MvcHtmlString NumericFor<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            return NumericFor(expression, null);
        }
        public MvcHtmlString NumericFor<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return new NumericBuilder<TModel>(_helper).Builder(expression, htmlAttributes);
        }
        #endregion

        #region ZipCodeFor
        public MvcHtmlString ZipCodeFor<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            return ZipCodeFor(expression, null);
        }
        public MvcHtmlString ZipCodeFor<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return new ZipCodeBuilder<TModel>(_helper).Builder(expression, htmlAttributes);
        }
        #endregion
    }
}
