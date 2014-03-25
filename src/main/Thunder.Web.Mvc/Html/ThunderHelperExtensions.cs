using System.Web.Mvc;

namespace Thunder.Web.Mvc.Html
{
    /// <summary>
    /// Thunder helper extension
    /// </summary>
    public static class ThunderHelperExtensions
    {
        /// <summary>
        /// Thunder html helper
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static Thunder<TModel> Thunder<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            return new Thunder<TModel>(htmlHelper);
        }
    }
}