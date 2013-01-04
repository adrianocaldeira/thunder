using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Thunder.ComponentModel.DataAnnotations;
using Thunder.Data;
using Thunder.Security;

namespace Manager.Models
{
    /// <summary>
    /// System user
    /// </summary>
    public class User : ActiveRecord<User, int>, IValidatableObject
    {
        private const string PasswordKey = "@9#7$5%W*&1WpC#@&2*%4$6#8@";

        /// <summary>
        /// Initialize new instance off <see cref="User"/>.
        /// </summary>
        public User()
        {
            State = State.Active;
            Functionalities = new List<Functionality>();
        }

        /// <summary>
        /// Get or set name
        /// </summary>
        [Display(Name = "Nome"), Required]
        public virtual string Name { get; set; }

        /// <summary>
        /// Get or set e-mail
        /// </summary>
        [Email(ErrorMessage = "E-mail informado é inválido.")]
        public virtual string Email { get; set; }

        /// <summary>
        /// Get or set login
        /// </summary>
        [Display(Name = "Login"), Required]
        public virtual string Login { get; set; }

        /// <summary>
        /// Get or set password
        /// </summary>
        [Display(Name = "Senha"), Required]
        public virtual string Password { get; set; }

        /// <summary>
        /// Get or set status
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Get or set functionalities
        /// </summary>
        public virtual IList<Functionality> Functionalities { get; set; }

        /// <summary>
        /// Encript password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string EncriptPassword(string password)
        {
            return password.Encrypt(PasswordKey);
        }

        /// <summary>
        /// Determines whether the specified object is valid.
        /// </summary>
        /// <returns>
        /// A collection that holds failed-validation information.
        /// </returns>
        /// <param name="validationContext">The validation context.</param>
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Functionalities == null || Functionalities.Count == 0)
            {
                yield return new ValidationResult("Selecione ao menos uma funcionalidade.", new[] { "Functionalities" });
            }
        }
    }
}