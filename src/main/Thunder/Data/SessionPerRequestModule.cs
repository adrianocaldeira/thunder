using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace Thunder.Data
{
    /// <summary>
    /// NHibernate module for bind and unbind session per request
    /// </summary>
    public class SessionPerRequestModule : IHttpModule
    {

        /// <summary>
        /// Init module
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            context.BeginRequest += BeginRequest;
            context.EndRequest += EndRequest;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Begin request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void BeginRequest(object sender, EventArgs e)
        {
            if (NeedPersistence(sender as HttpApplication))
            {
                SessionManager.Bind();
            }
        }

        /// <summary>
        /// End request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void EndRequest(object sender, EventArgs e)
        {
            if (NeedPersistence(sender as HttpApplication))
            {
                SessionManager.Unbind();
            }
        }

        /// <summary>
        /// Get static files
        /// </summary>
        private static IList<string> StaticFiles
        {
            get { return new List<string> { ".jpg", ".bmp", ".gif", ".png", ".css", ".js", ".swf", ".xap" }; }
        }

        /// <summary>
        /// Check need hibernate persistence
        /// </summary>
        /// <param name="application">Application</param>
        /// <returns>Need persistence</returns>
        public static bool NeedPersistence(HttpApplication application)
        {
            if (application == null || application.Context == null)
                return false;

            var extension = Path.GetExtension(application.Context.Request.PhysicalPath);
            return extension != null && !StaticFiles.Contains(extension.ToLower());
        }
    }
}
