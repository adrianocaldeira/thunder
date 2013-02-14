namespace Thunder.Web.Mvc
{
    /// <summary>
    /// Web View Page
    /// </summary>
    public abstract class WebViewPage : WebViewPage<dynamic>
    {
        /// <summary>
        /// Get thunder helper
        /// </summary>
        public new ThunderHelper Thunder { get; private set; }

        /// <summary>
        /// Init helpers
        /// </summary>
        public override void InitHelpers()
        {
            base.InitHelpers();
            Thunder = new ThunderHelper<object>(ViewContext, this, Html);
        }
    }
}
