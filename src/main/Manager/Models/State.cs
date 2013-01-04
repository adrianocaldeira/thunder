using Thunder.Data;

namespace Manager.Models
{
    /// <summary>
    /// State
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

        /// <summary>
        /// Get or set name
        /// </summary>
        public virtual string Name { get; set; }
    }
}