using System.Collections.Generic;
using Thunder.Web;

namespace Thunder
{
    /// <summary>
    /// Notify
    /// </summary>
    public class Notify
    {
        /// <summary>
        /// Initialize new instance of class <see cref="Notify"/>.
        /// </summary>
        public Notify()
        {
            Type = NotifyType.Success;
            Messages = new List<string>();
        }

        /// <summary>
        /// Initialize new instance of class <see cref="Notify"/>.
        /// </summary>
        /// <param name="message">Message</param>
        public Notify(string message) : this(new List<string> {message})
        {
        }

        /// <summary>
        /// Initialize new instance of class <see cref="Notify"/>.
        /// </summary>
        /// <param name="messages">Messages</param>
        public Notify(IList<string> messages) : this(NotifyType.Success, messages)
        {
        }

        /// <summary>
        /// Initialize new instance of class <see cref="Notify"/>.
        /// </summary>
        /// <param name="type"><see cref="NotifyType"/></param>
        /// <param name="message">Message</param>
        public Notify(NotifyType type, string message)
            : this(type, new List<string> {message})
        {
        }

        /// <summary>
        /// Initialize new instance of class <see cref="Notify"/>.
        /// </summary>
        /// <param name="type"><see cref="NotifyType"/></param>
        /// <param name="messages">Messages</param>
        public Notify(NotifyType type, IList<string> messages) : this()
        {
            Type = type;
            Messages = messages;
        }

        /// <summary>
        /// Get or set type
        /// </summary>
        public NotifyType Type { get; private set; }

        /// <summary>
        /// Get or set messsages
        /// </summary>
        public IList<string> Messages { get; private set; }
    }
}