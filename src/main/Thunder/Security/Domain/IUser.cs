using System.Collections.Generic;

namespace Thunder.Security.Domain
{
    /// <summary>
    /// User interface
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// Get or set user id
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Get or set user name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Get or set user login
        /// </summary>
        string Login { get; set; }

        /// <summary>
        /// Get or set user e-mail
        /// </summary>
        string Email { get; set; }

        /// <summary>
        /// Get or set areas that the user has access
        /// </summary>
        IList<IArea> Areas { get; set; }

        /// <summary>
        /// Get or set user password
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// Get or set indication whether the user is active
        /// </summary>
        bool Active { get; set; }
    }
}