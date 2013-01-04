using System.Collections.Generic;
using System.Linq;
using Thunder.Data;

namespace Manager.Models
{
    /// <summary>
    /// Functionality of system
    /// </summary>
    public class Functionality : ActiveRecord<Functionality, int>
    {
        /// <summary>
        /// Initiaize new instance of class <see cref="Functionality"/>.
        /// </summary>
        public Functionality()
        {
            Childs = new List<Functionality>();
        }

        /// <summary>
        /// Get or set parent funcionality
        /// </summary>
        public virtual Functionality Parent { get; set; }

        /// <summary>
        /// Get or set name
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Get or set description
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Get or set childs functionality
        /// </summary>
        public virtual IList<Functionality> Childs { get; set; }

        /// <summary>
        /// Get or set path
        /// </summary>
        public virtual string Path { get; set; }

        /// <summary>
        /// Check contains path
        /// </summary>
        /// <param name="path">Path</param>
        /// <returns>Contains</returns>
        public virtual bool Contains(string path)
        {
            if (!string.IsNullOrEmpty(Path))
            {
                return path.Contains(Path.Replace("~/", "")) ||
                       Childs.Any(child => path.Contains(child.Path.Replace("~/", "")));
            }

            return Childs.Any(child => path.Contains(child.Path.Replace("~/", "")));
        }
    }
}