namespace Thunder.Web.Mvc
{
    /// <summary>
    /// WebViewPage
    /// </summary>
    /// <typeparam name="TModel">Model type</typeparam>
    public abstract class WebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
        /// <summary>
        /// Get thunder helper
        /// </summary>
        public ThunderHelper<TModel> Thunder { get; private set; }

        /// <summary>
        /// Init helpers
        /// </summary>
        public override void InitHelpers()
        {
            base.InitHelpers();

            Thunder = new ThunderHelper<TModel>(ViewContext, this, Html);
        }
    }
}
