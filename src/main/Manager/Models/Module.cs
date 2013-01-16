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
        private IList<Functionality> _allFunctionalities;

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
        /// Recupera ou define funcionalidade padrão do módulo
        /// </summary>
        public virtual Functionality DefaultFunctionality
        {
            get { return Functionalities.SingleOrDefault(x => x.Default); }
        }

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
            return _allFunctionalities ?? (_allFunctionalities = AllFunctionalities(this));
        }

        /// <summary>
        /// Localiza módulo do usuário recursivamente
        /// </summary>
        /// <param name="user"><see cref="User"/></param>
        /// <param name="parent"><see cref="Module"/></param>
        /// <returns></returns>
        private static IEnumerable<Module> FindByUser(User user, Module parent)
        {
            var modules = new List<Module>();
            
            if(user.Profile.HasAccess(parent))
            {
                var module = new Module
                {
                    Id = parent.Id,
                    Name = parent.Name,
                    Order = parent.Order,
                    Description = parent.Description,
                    Created = parent.Created,
                    Updated = parent.Updated,
                    Functionalities = parent.Functionalities
                };

                foreach (var item in parent.Childs.SelectMany(child => FindByUser(user, child)))
                {
                    module.Childs.Add(item);
                }

                modules.Add(module);
            }

            return modules;
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

        /// <summary>
        /// Lista os módulos que o usuário possui acesso
        /// </summary>
        /// <param name="user">Usuário</param>
        /// <returns></returns>
        public static IList<Module> FindByUser(User user)
        {
            var modules = new List<Module>();

            foreach (var module in Parents())
            {
                modules.AddRange(FindByUser(user, module));
            }

            return modules;
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

        /// <summary>
        /// Módulo contém funcionalidade
        /// </summary>
        /// <param name="controllerName">Nome da controller</param>
        /// <param name="actionName">Nome da action</param>
        /// <returns>Contém</returns>
        public virtual bool Contains(string controllerName, string actionName)
        {
            return AllFunctionalities().Any(functionality => functionality.Controller.ToLower().Equals(controllerName.ToLower()) && functionality.Action.ToLower().Equals(actionName.ToLower()));
        }

        /// <summary>
        /// Clona módulo
        /// </summary>
        /// <returns><see cref="Module"/></returns>
        public virtual Module Clone()
        {
            return new Module
            {
                Id = Id,
                Name = Name,
                Order = Order,
                Description = Description,
                Created = Created,
                Updated = Updated,
                Functionalities = Functionalities,
                Childs = Childs,
                Parent = Parent
            };
        }
        #endregion
    }
}