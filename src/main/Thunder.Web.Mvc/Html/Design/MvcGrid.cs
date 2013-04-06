using System;
using System.IO;
using System.Web.Mvc;

namespace Thunder.Web.Mvc.Html.Design
{
    /// <summary>
    /// Mvc Grid
    /// </summary>
    public class MvcGrid : IDisposable
    {
        private readonly TextWriter _writer;
        private bool _disposed;

        /// <summary>
        /// Initialize new instance of <see cref="MvcGrid"/>.
        /// </summary>
        /// <param name="viewContext"><see cref="ViewContext"/></param>
        /// <exception cref="ArgumentNullException"></exception>
        public MvcGrid(ViewContext viewContext)
        {
            if (viewContext == null)
            {
                throw new ArgumentNullException("viewContext");
            }

            _writer = viewContext.Writer;
        }
        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            _disposed = true;
            _writer.Write("</div>");
        }
    }
}
