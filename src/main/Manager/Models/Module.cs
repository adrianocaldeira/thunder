using System.Collections.Generic;
using NHibernate.Criterion;
using Thunder.Data;
using System.Linq;

namespace Manager.Models
{
    /// <summary>
    /// Módulos do sistema
    /// </summary>
    public class Module : ActiveRecord<Module, int>
    {
        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="Module"/>.
        /// </summary>
        public Module()
        {
            Functionalities = new List<Functionality>();
            Childs = new List<Module>();
        }

        /// <summary>
        /// Recupera ou define nome do módulo
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Recupera ou define módulo pai
        /// </summary>
        public virtual Module Parent { get; set; }

        /// <summary>
        /// Recupera ou define descrição do módulo
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Recupera ou define ordem do módulo
        /// </summary>
        public virtual int Order { get; set; }

        /// <summary>
        /// Recupera ou define funcionalidades do módulo
        /// </summary>
        public virtual IList<Functionality> Functionalities { get; set; }

        /// <summary>
        /// Recupera ou define filhos do módulo
        /// </summary>
        public virtual IList<Module> Childs { get; set; }

        #region Private Static Methods

        /// <summary>
        /// Lista todas as funcionalidades do módulo e de seus filhos
        /// </summary>
        /// <param name="module"><see cref="Module"/></param>
        /// <returns></returns>
        private static IList<Functionality> AllFunctionalities(Module module)
        {
            var functionalities = module.Functionalities.ToList();

            foreach (var child in module.Childs)
            {
                functionalities.AddRange(AllFunctionalities(child));
            }

            return functionalities;
        }

        /// <summary>
        /// Lista todas as funcionalidades do módulo e de seus filhos
        /// </summary>
        /// <returns></returns>
        private IList<Functionality> AllFunctionalities()
        {
            return AllFunctionalities(this);
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Lista todos os módulos pais
        /// </summary>
        /// <returns></returns>
        public static IList<Module> Parents()
        {
            return Where(Restrictions.IsNull("Parent")).OrderBy(x => x.Order).ToList();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Módulo contém funcionalidade
        /// </summary>
        /// <param name="functionality"><see cref="Functionality"/></param>
        /// <returns>Contém</returns>
        public virtual bool Contains(Functionality functionality)
        {
            return AllFunctionalities().Contains(functionality);
        }

        #endregion
    }
}