using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Criterion;
using Thunder.ComponentModel.DataAnnotations;
using Thunder.Data;
using Thunder.Security;

namespace $rootnamespace$.Models
{
    /// <summary>
    /// Usuário do sistema
    /// </summary>
    public class User : ActiveRecord<User, int>
    {
        private const string PasswordKey = "@9#7$5%W*&1WpC#@&2*%4$6#8@";

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="User"/>.
        /// </summary>
        public User()
        {
            Status = Status.Active;
        }

        /// <summary>
        /// Recupera ou define perfil
        /// </summary>
        [Display(Name = "Perfil")]
        public virtual UserProfile Profile { get; set; }

        /// <summary>
        /// Recupera ou define nome
        /// </summary>
        [Required]
        [StringLength(100)]
        [Display(Name = "Nome")]
        public virtual string Name { get; set; }

        /// <summary>
        /// Recupera ou define e-mail
        /// </summary>
        [Email(ErrorMessage = "E-mail informado é inválido.")]
        [StringLength(100)]
        [Display(Name = "E-mail")]
        public virtual string Email { get; set; }

        /// <summary>
        /// Recupera ou define login
        /// </summary>
        [Required]
        [StringLength(20)]
        [Display(Name = "Login")]
        public virtual string Login { get; set; }

        /// <summary>
        /// Recupera ou define senha
        /// </summary>
        [Required]
        [Display(Name = "Senha")]
        [StringLength(10)]
        public virtual string Password { get; set; }

        /// <summary>
        /// Recupera ou define status
        /// </summary>
        public virtual Status Status { get; set; }

        /// <summary>
        /// Recupera senha descriptografada
        /// </summary>
        public virtual string PlanPassword
        {
            get { return Password.Decrypt(PasswordKey); }
        }

        #region Public Static Methods
        /// <summary>
        /// Encripta senha
        /// </summary>
        /// <param name="password">Senha</param>
        /// <returns>Senha criptografada</returns>
        public static string EncriptPassword(string password)
        {
            return password.Encrypt(PasswordKey);
        }

        /// <summary>
        /// Localiza usuário pelo login e senha
        /// </summary>
        /// <param name="login">Login</param>
        /// <param name="password">Senha</param>
        /// <returns><see cref="User"/></returns>
        public static User Find(string login, string password)
        {
            using (var transaction = Session.BeginTransaction())
            {
                var user = Session.GetNamedQuery("users-find-by-login-password")
                    .SetString("login", login.ToLower())
                    .SetString("password", EncriptPassword(password))
                    .SetEntity("status", Status.Active)
                    .UniqueResult<User>();

                transaction.Commit();

                return user;
            }
        }

        /// <summary>
        /// Possui acesso a uma funcionalidade
        /// </summary>
        /// <param name="id">Código</param>
        /// <param name="controllerName">Nome da Controller</param>
        /// <param name="actionName">Nome da Action</param>
        /// <param name="httpMethod">Método Http</param>
        /// <returns>Possui acesso</returns>
        public static bool AllowAccess(int id, string controllerName, string actionName, string httpMethod)
        {
            using (var transaction = Session.BeginTransaction())
            {
                var functionalities = Session.GetNamedQuery("users-allow-access")
                    .SetInt32("id", id)
                    .SetString("controllerName", controllerName.ToLower())
                    .SetString("actionName", actionName.ToLower())
                    .List<Functionality>();

                transaction.Commit();

                return functionalities.Any(functionality => functionality.HttpMethod.ToLower().Contains(httpMethod.ToLower()));
            }
        }

        /// <summary>
        /// Verifica se modelo é válido
        /// </summary>
        /// <param name="modelState"><see cref="ModelStateDictionary"/></param>
        /// <returns>Válido</returns>
        public virtual bool IsValid(ModelStateDictionary modelState)
        {
            if (modelState.IsValid)
            {
                if (Exist(Id, Restrictions.Eq(Projections.SqlFunction("lower", NHibernateUtil.String,
                    Projections.Property("Login")), Login.ToLower())))
                {
                    modelState.AddModelError("Login", "O login informado já existe.");
                }

                if (!string.IsNullOrEmpty(Email) && Exist(Id, Restrictions.Eq(Projections.SqlFunction("lower",
                    NHibernateUtil.String, Projections.Property("Email")), Login.ToLower())))
                {
                    modelState.AddModelError("Login", "O e-mail informado já existe.");
                }

                return modelState.IsValid;
            }

            return modelState.IsValid;
        }
        #endregion
    }
}