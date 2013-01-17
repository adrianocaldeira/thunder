using System.ComponentModel.DataAnnotations;
using Thunder.Data;

namespace Manager.Models
{
    /// <summary>
    /// Status
    /// </summary>
    public class State : ActiveRecord<State, short>
    {
        /// <summary>
        /// Ativo
        /// </summary>
        public static State Active = new State {Id = 1, Name = "Ativo"};

        /// <summary>
        /// Inativo
        /// </summary>
        public static State Inactive = new State {Id = 2, Name = "Inativo"};

        /// <summary>
        /// Recupera ou define código
        /// </summary>
        [Display(Name = "Status"), Required]
        public new virtual short Id { get { return base.Id; } set { base.Id = value; } }

        /// <summary>
        /// Recupera ou define nome
        /// </summary>
        public virtual string Name { get; set; }
    }
}