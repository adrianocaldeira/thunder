using System;
using System.Configuration;
using System.Resources;

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
            Text = ConfigurationManager.AppSettings[key];
            
            if (string.IsNullOrEmpty(Text))
            {
                Text = key;
            }
        }

        /// <summary>
        /// Initialize a new instance of the class <see cref="Message"/>.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="resourceManager"><see cref="ResourceManager"/></param>
        public Message(string key, ResourceManager resourceManager)
        {
            if (resourceManager == null)
                throw new ArgumentException("Parameter is not informed.", "resourceManager");

            Text = resourceManager.GetString(key);

            if (string.IsNullOrEmpty(Text))
            {
                Text = key;
            }
        }

        /// <summary>
        /// Initialize a new instance of the class <see cref="Message"/>.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="field">Value</param>
        /// <param name="resourceManager"><see cref="ResourceManager"/></param>
        public Message(string key, string field, ResourceManager resourceManager) : this(key, resourceManager)
        {
            Field = field;
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