using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NHibernate.Criterion;
using Thunder.ComponentModel.DataAnnotations;
using Thunder.Data;
using System.Linq;

namespace Manager.Models
{
    /// <summary>
    /// Perfil de usuário
    /// </summary>
    public class UserProfile : ActiveRecord<UserProfile, int>
    {
        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="UserProfile"/>.
        /// </summary>
        public UserProfile()
        {
            State = State.Active;
            Functionalities = new List<Functionality>();
        }

        /// <summary>
        /// Recupera ou define código
        /// </summary>
        [Display(Name = "Perfil")]
        public new virtual int Id { get; set; }

        /// <summary>
        /// Recuper ou define nome
        /// </summary>
        [Display(Name = "Nome"), Required]
        public virtual string Name { get; set; }

        /// <summary>
        /// Recuper aou define funcionalidades
        /// </summary>
        [ListRequired(ErrorMessage = "Selecione ao menos uma funcionalidade.")]
        public virtual IList<Functionality> Functionalities { get; set; }

        /// <summary>
        /// Recupera ou define estado do perfil
        /// </summary>
        public virtual State State { get; set; }

        #region Public Static Methods

        /// <summary>
        /// Localiza perfis de usuário pelo estado
        /// </summary>
        /// <param name="state"><see cref="State"/></param>
        /// <returns></returns>
        public static IList<UserProfile> FindByState(State state)
        {
            return Where(Restrictions.Eq("State", state)).OrderBy(x => x.Name).ToList();
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

        /// <summary>
        /// Pode remover perfil de usuário 
        /// </summary>
        /// <returns>Pode remover</returns>
        public virtual bool CanDelete()
        {
            using (var transaction = Session.BeginTransaction())
            {
                var list = Session.GetNamedQuery("user-profiles-can-remove")
                    .SetEntity("profile", this)
                    .List();

                transaction.Commit();

                return list.Count == 0;
            }
        }
        #endregion


    }
}