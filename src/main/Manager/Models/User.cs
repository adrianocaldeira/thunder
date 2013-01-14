using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Thunder.ComponentModel.DataAnnotations;
using Thunder.Data;
using Thunder.Security;

namespace Manager.Models
{
    /// <summary>
    /// System user
    /// </summary>
    public class User : ActiveRecord<User, int>
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
        [ListRequired(ErrorMessage = "Selecione ao menos uma funcionalidade.")]
        public virtual IList<Functionality> Functionalities { get; set; }

        #region Public Static Methods

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
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static User Find(string login, string password)
        {
            using (var transaction = Session.BeginTransaction())
            {
                var user = Session.GetNamedQuery("users-find-by-login-password")
                    .SetString("login", login.ToLower())
                    .SetString("password", EncriptPassword(password))
                    .UniqueResult<User>();

                transaction.Commit();

                return user;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Possui acesso ao módulo do sistema
        /// </summary>
        /// <param name="module"><see cref="Module"/></param>
        /// <returns>Possui acesso</returns>
        public virtual bool HasAccess(Module module)
        {
            return Functionalities.Any(module.Contains);
        }

        /// <summary>
        /// Possui acesso a funcionalidade de um módulo
        /// </summary>
        /// <param name="functionality"><see cref="Functionality"/></param>
        /// <returns>Possui acesso</returns>
        public virtual bool HasAccess(Functionality functionality)
        {
            return Functionalities.Contains(functionality);
        }

        #endregion
    }
}