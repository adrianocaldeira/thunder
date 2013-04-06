using System;
using System.IO;
using System.Web.Mvc;

namespace Thunder.Web.Mvc.Html.Design.SimplaAdmin
{
    ///<summary>
    /// Mvc content box
    ///</summary>
    public class MvcContentBox : IDisposable
    {
        private readonly TextWriter _writer;
        private bool _disposed;

        /// <summary>
        /// Initialize new instance of <see cref="MvcContentBox"/>.
        /// </summary>
        /// <param name="viewContext"></param>
        public MvcContentBox(ViewContext viewContext)
        {
            if (viewContext == null)
            {
                throw new ArgumentNullException("viewContext");
            }

            _writer = viewContext.Writer;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
                _writer.Write("</div>");
                _writer.Write("<div class=\"clear\"></div>");
                _writer.Write("</div>");
            }
        }
    }
}