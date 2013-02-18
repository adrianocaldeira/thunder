using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Criterion;
using Thunder.Data;

namespace $rootnamespace$.Models
{
    /// <summary>
    /// Notícia
    /// </summary>
    public class News : ActiveRecord<News, int>
    {
        /// <summary>
        /// Recupera ou define responsável pelo cadastro
        /// </summary>
        public virtual User Sponsor { get; set; }

        /// <summary>
        /// Recupera ou define título
        /// </summary>
        [Display(Name = "Título"), Required]
        public virtual string Title { get; set; }

        /// <summary>
        /// Recupera ou define data
        /// </summary>
        [Display(Name = "Data"), Required]
        public virtual DateTime Date { get; set; }

        /// <summary>
        /// Recupera ou define chamada
        /// </summary>
        [Display(Name = "Chamada"), Required]
        public virtual string Call { get; set; }

        /// <summary>
        /// Recupera ou define autor
        /// </summary>
        [Display(Name = "Autor")]
        public virtual string Author { get; set; }

        /// <summary>
        /// Recupera ou define conteúdo
        /// </summary>
        [Display(Name = "Conteúdo"), Required]
        [UIHint("TinyMceSimple"), AllowHtml]
        public virtual string Content { get; set; }

        #region Public Methods
        /// <summary>
        /// Modelo válido
        /// </summary>
        /// <param name="modelState"><see cref="ModelStateDictionary"/></param>
        /// <returns></returns>
        public virtual bool IsValid(ModelStateDictionary modelState)
        {
            if (modelState.IsValid)
            {
                if (Exist(Id, Restrictions.Eq(Projections.SqlFunction("lower", NHibernateUtil.String, Projections.Property("Title")), Title.ToLower())))
                {
                    modelState.AddModelError("Title", "Já existe uma notícia cadastrada com o título informado.");
                }
            }

            return modelState.IsValid;
        }
        #endregion
    }
}