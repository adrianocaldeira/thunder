using System.Configuration;

namespace Thunder
{
    /// <summary>
    /// Message
    /// </summary>
    public class Message
    {
        #region Constructors

        /// <summary>
        /// Initialize a new instance of the class <see cref="Message"/>.
        /// </summary>
        /// <param name="key">Key</param>
        public Message(string key)
        {
            Text = string.IsNullOrEmpty(ConfigurationManager.AppSettings[key])
                       ? key
                       : ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// Initialize a new instance of the class <see cref="Message"/>.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="field">Value</param>
        public Message(string key, string field)
            : this(key)
        {
            Field = field;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get text message
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Get field reference
        /// </summary>
        public string Field { get; private set; }

        #endregion
    }
}