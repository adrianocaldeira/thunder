using System.Web.Mvc;
using System.Web.Routing;

namespace Thunder.Web.Mvc.Html.Design
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
        ///<param name="expression">Expression</param>
        ///<param name="labelText">Label text</param>
        ///<param name="required">Required</param>
        ///<returns>Label tag</returns>
        public static MvcHtmlString Label(this HtmlHelper helper, string expression, string labelText, bool required)
        {
         
            return helper.Label(expression, labelText, required, null);
        }

        /// <summary>
        /// Label
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <param name="expression">Expression</param>
        /// <param name="labelText">Label text</param>
        /// <param name="required">Required</param>
        /// <param name="htmlAttributes">Html attributes</param>
        /// <returns></returns>
        public static MvcHtmlString Label(this HtmlHelper helper, string expression, string labelText, bool required, object htmlAttributes)
        {
            var builder = new TagBuilder("label");

            builder.Attributes.Add("for", expression);
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
