using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace Thunder.Web.Mvc.Html.Design.SimplaAdmin
{
    /// <summary>
    /// Label extensions
    /// </summary>
    public static class LabelExtensions
    {
        ///<summary>
        /// Label
        ///</summary>
        ///<param name="helper">Helper</param>
        ///<param name="forName">For Name</param>
        ///<param name="labelText">Label text</param>
        ///<param name="required">Required</param>
        /// <returns><see cref="MvcHtmlString"/></returns>
        public static MvcHtmlString Label(this HtmlHelper helper, string forName, string labelText, bool required)
        {
         
            return helper.Label(forName, labelText, required, null);
        }

        /// <summary>
        /// Label For
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <param name="expression">Expression</param>
        /// <param name="labelText">Label Text</param>
        /// <param name="required">Required</param>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <returns><see cref="MvcHtmlString"/></returns>
        public static MvcHtmlString LabelFor<TModel, TProperty>(this HtmlHelper helper, Expression<Func<TModel, TProperty>> expression, string labelText, bool required)
        {
            return helper.Label(ExpressionHelper.GetExpressionText(expression), labelText, required, null);
        }

        /// <summary>
        /// Label For
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <param name="expression">Expression</param>
        /// <param name="required">Required</param>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <returns><see cref="MvcHtmlString"/></returns>
        public static MvcHtmlString LabelFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, bool required)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var forName = ExpressionHelper.GetExpressionText(expression);
	        var labelText = metadata.DisplayName ?? metadata.PropertyName ?? forName.Split('.').Last();

            return helper.Label(forName, labelText, required, null);
        }

        /// <summary>
        /// Label
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <param name="forName">For name</param>
        /// <param name="labelText">Label text</param>
        /// <param name="required">Required</param>
        /// <param name="htmlAttributes">Html attributes</param>
        /// <returns><see cref="MvcHtmlString"/></returns>
        public static MvcHtmlString Label(this HtmlHelper helper, string forName, string labelText, bool required, object htmlAttributes)
        {
            var builder = new TagBuilder("label");

            builder.Attributes.Add("for", forName.Replace(".", builder.IdAttributeDotReplacement));
            builder.SetInnerText(labelText);

            if (required)
            {
                var span = new TagBuilder("span");

                span.AddCssClass("required");
                span.SetInnerText("*");

                builder.InnerHtml += "&nbsp;";
                builder.InnerHtml += span;
            }

            if (htmlAttributes != null)
            {
                builder.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);
            }

            return MvcHtmlString.Create(builder.ToString());
        }
    }
}
