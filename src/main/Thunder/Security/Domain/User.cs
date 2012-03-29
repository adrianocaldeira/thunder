using System.Collections.Generic;

namespace Thunder.Security.Domain
{
    /// <summary>
    /// User
    /// </summary>
    public class User : IUser
    {
        /// <summary>
        /// Initialize a new instance of the class <see cref="User"/>.
        /// </summary>
        public User()
        {
            Areas = new List<IArea>();
        }

        #region Implementation of IUser

        /// <summary>
        /// Get or set user id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Get or set user name
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Get or set user login
        /// </summary>
        public virtual string Login { get; set; }

        /// <summary>
        /// Get or set user e-mail
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// Get or set areas that the user has access
        /// </summary>
        public virtual IList<IArea> Areas { get; set; }

        /// <summary>
        /// Get or set user password
        /// </summary>
        public virtual string Password { get; set; }

        /// <summary>
        /// Get or set indication whether the user is active
        /// </summary>
        public virtual bool Active { get; set; }

        #endregion
    }
}