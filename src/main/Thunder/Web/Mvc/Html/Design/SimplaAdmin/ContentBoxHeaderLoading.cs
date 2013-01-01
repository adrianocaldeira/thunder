namespace Thunder.Web.Mvc.Html.Design.SimplaAdmin
{
    /// <summary>
    /// Content box header loading
    /// </summary>
    public class ContentBoxHeaderLoading
    {
        /// <summary>
        /// Initialize new instance of <see cref="ContentBoxHeaderLoading"/>.
        /// </summary>
        public ContentBoxHeaderLoading()
        {
            Class = "loading";
            ImagePath = "~/content/images/loading.gif";
        }

        /// <summary>
        /// Get or set id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Get or set image path
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// Get or set class
        /// </summary>
        public string Class { get; set; }
    }
}