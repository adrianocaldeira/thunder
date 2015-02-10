using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using Thunder.Collections;
using Thunder.Web.Mvc.Html.Cnpj;
using Thunder.Web.Mvc.Html.Cpf;
using Thunder.Web.Mvc.Html.Currency;
using Thunder.Web.Mvc.Html.Date;
using Thunder.Web.Mvc.Html.Grid;
using Thunder.Web.Mvc.Html.Image;
using Thunder.Web.Mvc.Html.JavaScript;
using Thunder.Web.Mvc.Html.Notify;
using Thunder.Web.Mvc.Html.Numeric;
using Thunder.Web.Mvc.Html.Pagination;
using Thunder.Web.Mvc.Html.Password;
using Thunder.Web.Mvc.Html.Phone;
using Thunder.Web.Mvc.Html.StyleSheet;
using Thunder.Web.Mvc.Html.TextArea;
using Thunder.Web.Mvc.Html.TextBox;
using Thunder.Web.Mvc.Html.ZipCode;

namespace Thunder.Web.Mvc.Html
{
    /// <summary>
    /// Thunder html controls
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class Thunder<TModel>
    {
        private readonly HtmlHelper<TModel> _helper;

        internal Thunder(HtmlHelper<TModel> helper)
        {
            _helper = helper;
        }

        #region TextBoxFor
        /// <summary>
        /// TextBoxFor
        /// </summary>
        /// <param name="expression"></param>
        /// <typeparam name="TProperty"></typeparam>
        /// <returns></returns>
        public MvcHtmlString TextBoxFor<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            return TextBoxFor(expression, null);
        }
        /// <summary>
        /// TextBoxFor
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public MvcHtmlString TextBoxFor<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return new TextBoxBuilder<TModel>(_helper).Builder(expression, htmlAttributes);
        }
        #endregion

        #region TextAreaFor
        /// <summary>
        /// TextAreaFor
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public MvcHtmlString TextAreaFor<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            return TextAreaFor(expression, null);
        }
        /// <summary>
        /// TextAreaFor
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public MvcHtmlString TextAreaFor<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return new TextAreaBuilder<TModel>(_helper).Builder(expression, htmlAttributes);
        }
        #endregion

        #region PasswordFor
        /// <summary>
        /// PasswordFor
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public MvcHtmlString PasswordFor<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            return PasswordFor(expression, null);
        }
        /// <summary>
        /// PasswordFor
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public MvcHtmlString PasswordFor<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return new PasswordBuilder<TModel>(_helper).Builder(expression, htmlAttributes);
        }
        #endregion

        #region CnpjFor
        /// <summary>
        /// CnpjFor
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public MvcHtmlString CnpjFor<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            return CnpjFor(expression, null);
        }
        /// <summary>
        /// CnpjFor
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public MvcHtmlString CnpjFor<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return new CnpjBuilder<TModel>(_helper).Builder(expression, htmlAttributes);
        }
        #endregion

        #region CpfFor
        /// <summary>
        /// CpfFor
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public MvcHtmlString CpfFor<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            return CpfFor(expression, null);
        }
        /// <summary>
        /// CpfFor
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public MvcHtmlString CpfFor<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return new CpfBuilder<TModel>(_helper).Builder(expression, htmlAttributes);
        }
        #endregion

        #region CurrencyFor
        /// <summary>
        /// CurrencyFor
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public MvcHtmlString CurrencyFor<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            return CurrencyFor(expression, null);
        }
        /// <summary>
        /// CurrencyFor
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public MvcHtmlString CurrencyFor<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return new CurrencyBuilder<TModel>(_helper).Builder(expression, htmlAttributes);
        }
        #endregion

        #region DateFor
        /// <summary>
        /// DateFor
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public MvcHtmlString DateFor<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            return DateFor(expression, null);
        }
        /// <summary>
        /// DateFor
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public MvcHtmlString DateFor<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return new DateBuilder<TModel>(_helper).Builder(expression, htmlAttributes);
        }
        #endregion

        #region PhoneFor
        /// <summary>
        /// PhoneFor
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public MvcHtmlString PhoneFor<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            return PhoneFor(expression, null);
        }
        /// <summary>
        /// PhoneFor
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public MvcHtmlString PhoneFor<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return new PhoneBuilder<TModel>(_helper).Builder(expression, htmlAttributes);
        }
        #endregion

        #region NumericFor
        /// <summary>
        /// NumericFor
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public MvcHtmlString NumericFor<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            return NumericFor(expression, null);
        }
        /// <summary>
        /// NumericFor
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public MvcHtmlString NumericFor<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return new NumericBuilder<TModel>(_helper).Builder(expression, htmlAttributes);
        }
        #endregion

        #region ZipCodeFor
        /// <summary>
        /// ZipCodeFor
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public MvcHtmlString ZipCodeFor<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            return ZipCodeFor(expression, null);
        }
        /// <summary>
        /// ZipCodeFor
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public MvcHtmlString ZipCodeFor<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return new ZipCodeBuilder<TModel>(_helper).Builder(expression, htmlAttributes);
        }
        #endregion

        #region Grid
        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public MvcGrid Grid(string url)
        {
            return Grid(url, 15);
        }
        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="url"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public MvcGrid Grid(string url, int pageSize)
        {
            return Grid(url, pageSize, null);
        }
        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="url"></param>
        /// <param name="pageSize"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public MvcGrid Grid(string url, int pageSize, object htmlAttributes)
        {
            return new GridBuilder<TModel>(_helper).Builder(url, pageSize, htmlAttributes);
        }
        #endregion

        #region GridSort
        /// <summary>
        /// GridSort
        /// </summary>
        /// <param name="text"></param>
        /// <param name="column"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public MvcHtmlString GridSort(string text, string column, Model.Filter filter)
        {
            return GridSort(text, column, filter, null);
        }
        /// <summary>
        /// GridSort
        /// </summary>
        /// <param name="text"></param>
        /// <param name="column"></param>
        /// <param name="filter"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public MvcHtmlString GridSort(string text, string column, Model.Filter filter, object htmlAttributes)
        {
            return new GridSortBuilder().Builder(text, column, filter, htmlAttributes);
        }
        #endregion

        #region Notify
        /// <summary>
        /// Notify
        /// </summary>
        /// <returns></returns>
        public MvcHtmlString Notify()
        {
            return Notify(false);
        }
        /// <summary>
        /// Notify
        /// </summary>
        /// <param name="showCloseButton"></param>
        /// <returns></returns>
        public MvcHtmlString Notify(bool showCloseButton)
        {
            return Notify(showCloseButton, null);
        }
        /// <summary>
        /// Notify
        /// </summary>
        /// <param name="showCloseButton"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public MvcHtmlString Notify(bool showCloseButton, object htmlAttributes)
        {
            return Notify(_helper.ViewData[Constants.ViewData.Notify] as Thunder.Notify ?? new Thunder.Notify(), 
                showCloseButton, htmlAttributes);
        }
        /// <summary>
        /// Notify
        /// </summary>
        /// <param name="notifyType"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public MvcHtmlString Notify(NotifyType notifyType, string message)
        {
            return Notify(notifyType, message, false);
        }
        /// <summary>
        /// Notify
        /// </summary>
        /// <param name="notifyType"></param>
        /// <param name="message"></param>
        /// <param name="showCloseButton"></param>
        /// <returns></returns>
        public MvcHtmlString Notify(NotifyType notifyType, string message, bool showCloseButton)
        {
            return Notify(notifyType, message, showCloseButton, null);
        }
        /// <summary>
        /// Notify
        /// </summary>
        /// <param name="notifyType"></param>
        /// <param name="message"></param>
        /// <param name="showCloseButton"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public MvcHtmlString Notify(NotifyType notifyType, string message, bool showCloseButton, object htmlAttributes)
        {
            return Notify(notifyType, new List<string>{message}, showCloseButton, htmlAttributes);
        }
        /// <summary>
        /// Notify
        /// </summary>
        /// <param name="notifyType"></param>
        /// <param name="messages"></param>
        /// <returns></returns>
        public MvcHtmlString Notify(NotifyType notifyType, IList<string> messages)
        {
            return Notify(notifyType, messages, false);
        }
        /// <summary>
        /// Notify
        /// </summary>
        /// <param name="notifyType"></param>
        /// <param name="messages"></param>
        /// <param name="showCloseButton"></param>
        /// <returns></returns>
        public MvcHtmlString Notify(NotifyType notifyType, IList<string> messages, bool showCloseButton)
        {
            return Notify(notifyType, messages, showCloseButton, null);
        }
        /// <summary>
        /// Notify
        /// </summary>
        /// <param name="notifyType"></param>
        /// <param name="messages"></param>
        /// <param name="showCloseButton"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public MvcHtmlString Notify(NotifyType notifyType, IList<string> messages, bool showCloseButton, object htmlAttributes)
        {
            return Notify(new Thunder.Notify(notifyType, messages), showCloseButton, htmlAttributes);
        }
        /// <summary>
        /// Notify
        /// </summary>
        /// <param name="notify"></param>
        /// <param name="showCloseButton"></param>
        /// <returns></returns>
        public MvcHtmlString Notify(Thunder.Notify notify, bool showCloseButton)
        {
            return Notify(notify, showCloseButton, null);
        }
        /// <summary>
        /// Notify
        /// </summary>
        /// <param name="notify"></param>
        /// <param name="showCloseButton"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public MvcHtmlString Notify(Thunder.Notify notify, bool showCloseButton, object htmlAttributes)
        {
            if (_helper.ViewContext.HttpContext.Session != null && _helper.ViewContext.HttpContext.Session[Constants.ViewData.Notify] != null)
            {
                _helper.ViewContext.HttpContext.Session.Remove(Constants.ViewData.Notify);
            }
            
            return new NotifyBuilder().Builder(notify, showCloseButton, htmlAttributes);
        }
        #endregion

        #region Image
        /// <summary>
        /// Image
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public MvcHtmlString Image(string url)
        {
            return Image(url, null);
        }
        /// <summary>
        /// Image
        /// </summary>
        /// <param name="url"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public MvcHtmlString Image(string url, object htmlAttributes)
        {
            return new ImageBuilder<TModel>(_helper).Builder(url, htmlAttributes);
        }
        #endregion

        #region StyleSheet
        /// <summary>
        /// StyleSheet
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public MvcHtmlString StyleSheet(string url)
        {
            return StyleSheet(url, null);
        }
        /// <summary>
        /// StyleSheet
        /// </summary>
        /// <param name="url"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public MvcHtmlString StyleSheet(string url, object htmlAttributes)
        {
            return new StyleSheetBuilder<TModel>(_helper).Builder(url, htmlAttributes);
        }
        #endregion

        #region JavaScript
        /// <summary>
        /// JavaScript
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public MvcHtmlString JavaScript(string url)
        {
            return JavaScript(url, null);
        }
        /// <summary>
        /// JavaScript
        /// </summary>
        /// <param name="url"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public MvcHtmlString JavaScript(string url, object htmlAttributes)
        {
            return new JavaScriptBuilder<TModel>(_helper).Builder(url, htmlAttributes);
        }
        #endregion

        #region JavaScript
        /// <summary>
        /// Pagination
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public MvcHtmlString Pagination<T>(IPaging<T> source)
        {
            return Pagination(source, null);
        }
        /// <summary>
        /// Pagination
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public MvcHtmlString Pagination<T>(IPaging<T> source, Func<int, string> url)
        {
            return Pagination(source, url, 5);
        }
        /// <summary>
        /// Pagination
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="url"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public MvcHtmlString Pagination<T>(IPaging<T> source, Func<int, string> url, object htmlAttributes)
        {
            return Pagination(source, url, 5, htmlAttributes);
        }
        /// <summary>
        /// Pagination
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public MvcHtmlString Pagination<T>(IPaging<T> source, int size)
        {
            return Pagination(source, null, size);
        }
        /// <summary>
        /// Pagination
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="size"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public MvcHtmlString Pagination<T>(IPaging<T> source, int size, object htmlAttributes)
        {
            return Pagination(source, null, size, htmlAttributes);
        }
        /// <summary>
        /// Pagination
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="url"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public MvcHtmlString Pagination<T>(IPaging<T> source, Func<int, string> url, int size)
        {
            return Pagination(source, url, size, null);
        }
        /// <summary>
        /// Pagination
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="url"></param>
        /// <param name="size"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public MvcHtmlString Pagination<T>(IPaging<T> source, Func<int, string> url, int size, object htmlAttributes)
        {
            return new PaginationBuilder().Builder(source, url, size, htmlAttributes);
        }
        #endregion
    }
}
