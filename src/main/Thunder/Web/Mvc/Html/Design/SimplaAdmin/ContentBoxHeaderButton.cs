namespace Thunder.Web.Mvc.Html.Design.SimplaAdmin
{
    /// <summary>
    /// Content box tab
    /// </summary>
    public class ContentBoxHeaderButton
    {
        /// <summary>
        /// Initialize new instance of <see cref="ContentBoxHeaderButton"/>.
        /// </summary>
        public ContentBoxHeaderButton()
        {
            Url = "~/";
            Class = "button";
        }
        /// <summary>
        /// Get or set text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Get or set title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Get or set url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Get or set class
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// Get or set html attributes
        /// </summary>
        public object HtmlAttributes { get; set; }
    }
}