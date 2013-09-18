using System.Web.Mvc;

namespace Thunder.Web.Mvc.Html
{
    public static class ThunderHelperExtensions
    {
        public static Thunder<TModel> Thunder<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            return new Thunder<TModel>(htmlHelper);
        }
    }
}