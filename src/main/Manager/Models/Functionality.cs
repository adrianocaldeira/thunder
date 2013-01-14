using System.Net;
using Thunder.Data;

namespace Manager.Models
{
    /// <summary>
    /// Funcionalida de um módulo do sistema
    /// </summary>
    public class Functionality : ActiveRecord<Functionality, int>
    {
        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="Functionality"/>.
        /// </summary>
        public Functionality()
        {
            HttpMethod = WebRequestMethods.Http.Get;
        }

        /// <summary>
        /// Recupera ou define nome
        /// </summary>
        public virtual Module Module { get; set; }

        /// <summary>
        /// Recupera ou define nome
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Recupera ou define descrição
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Recupera ou define ação
        /// </summary>
        public virtual string Action { get; set; }

        /// <summary>
        /// Recupera ou define controlador
        /// </summary>
        public virtual string Controller { get; set; }

        /// <summary>
        /// Recupera ou define método http
        /// </summary>
        public virtual string HttpMethod { get; set; }
    }
}