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
        /// Active
        /// </summary>
        public static State Active = new State {Id = 1, Name = "Ativo"};

        /// <summary>
        /// Inactive
        /// </summary>
        public static State Inactive = new State {Id = 2, Name = "Inativo"};

        [Display(Name = "Status"), Required]
        public new virtual short Id { get { return base.Id; } set { base.Id = value; } }

        /// <summary>
        /// Get or set name
        /// </summary>
        public virtual string Name { get; set; }
    }
}