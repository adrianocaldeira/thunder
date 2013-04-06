using System.Collections.Generic;

namespace Thunder.Web.Mvc.Html.Design.SimplaAdmin
{
    /// <summary>
    /// Content box header
    /// </summary>
    public class ContentBoxHeader
    {
        /// <summary>
        /// Initialize new instance of <see cref="ContentBoxHeader"/>.
        /// </summary>
        public ContentBoxHeader()
        {
            Tabs = new List<ContentBoxHeaderTab>();
            Buttons = new List<ContentBoxHeaderButton>();
        }

        /// <summary>
        /// Get or set title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Get or set required
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// Get or set loading
        /// </summary>
        public ContentBoxHeaderLoading Loading { get; set; }

        /// <summary>
        /// Get or set tab
        /// </summary>
        public IList<ContentBoxHeaderTab> Tabs { get; set; }

        /// <summary>
        /// Get or set buttons
        /// </summary>
        public IList<ContentBoxHeaderButton> Buttons { get; set; }
    }
}