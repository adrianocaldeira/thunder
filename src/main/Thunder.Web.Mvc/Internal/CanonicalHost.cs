using System.Configuration;

namespace Thunder.Web.Mvc.Internal
{
    /// <summary>
    /// Reads the optional canonical host configured by the application, allowing absolute URL
    /// generation to rely on a trusted, application-defined authority instead of the incoming
    /// request's Host header (mitigates host header poisoning).
    /// </summary>
    internal static class CanonicalHost
    {
        private const string AppSettingKey = "Thunder.Web.Mvc.CanonicalHost";

        private static string _value;
        private static bool _loaded;

        /// <summary>
        /// Gets the canonical host configured via the "Thunder.Web.Mvc.CanonicalHost" application
        /// setting, or <c>null</c> when the setting is absent or empty. The feature is opt-in: when
        /// this returns <c>null</c>, callers must keep relying on the current request's own authority.
        /// Any trailing slash in the configured value is trimmed, so callers composing URLs by simple
        /// concatenation (authority + "/relative/path") never end up with a double slash.
        /// The value is read once and cached in a static field for the lifetime of the application domain.
        /// </summary>
        public static string Value
        {
            get
            {
                if (!_loaded)
                {
                    var configured = ConfigurationManager.AppSettings[AppSettingKey];
                    _value = string.IsNullOrEmpty(configured) ? null : configured.TrimEnd('/');
                    _loaded = true;
                }

                return _value;
            }
        }
    }
}
