using log4net;

namespace Thunder
{
    /// <summary>
    /// Logging
    /// </summary>
    public static class Logging
    {
        /// <summary>
        /// Write in Application logger configuration of log4net
        /// </summary>
        public static ILog Write { get; private set; }

        /// <summary>
        /// Initialize new instance of <see cref="Logging"/>.
        /// </summary>
        static Logging()
        {
            Write = LogManager.GetLogger("Application");
        }
    }
}
