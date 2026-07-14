using System;
using System.Collections.Generic;

namespace Thunder.Data
{
    /// <summary>
    /// Persist object
    /// </summary>
    /// <typeparam name="T">Type class</typeparam>
    /// <typeparam name="TKey">Type key</typeparam>
    public class Persist<T, TKey> : ICreatedAndUpdatedProperty where T : class
    {
        private int? _cachedHashCode;

        /// <summary>
        /// Get or set id
        /// </summary>
        public virtual TKey Id { get; set; }

        /// <summary>
        /// Get or set created date
        /// </summary>
        public virtual DateTime Created { get; set; }

        /// <summary>
        /// Get or set updated date
        /// </summary>
        public virtual DateTime Updated { get; set; }

        /// <summary>
        /// Indica se o objeto ainda não foi persistido (chave com valor padrão do tipo).
        /// </summary>
        /// <returns><c>true</c> se o objeto é novo (transiente); caso contrário, <c>false</c>.</returns>
        public virtual bool IsNew()
        {
            return EqualityComparer<TKey>.Default.Equals(Id, default(TKey));
        }

        /// <summary>
        /// Compara a igualdade pela identidade (Id) e pelo tipo da entidade.
        /// Objetos transientes (novos) só são iguais por referência.
        /// </summary>
        /// <param name="obj">Objeto a comparar.</param>
        /// <returns><c>true</c> se representam a mesma entidade persistida; caso contrário, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            var other = obj as T;
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;
            var p = other as Persist<T, TKey>;
            if (p == null || IsNew() || p.IsNew()) return false;
            return EqualityComparer<TKey>.Default.Equals(Id, p.Id);
        }

        /// <summary>
        /// Retorna um hash code estável: calculado na primeira chamada e mantido em cache,
        /// para não mudar quando o Id for atribuído após a persistência.
        /// </summary>
        /// <returns>Hash code do objeto.</returns>
        public override int GetHashCode()
        {
            return _cachedHashCode ??=
                IsNew() ? base.GetHashCode() : EqualityComparer<TKey>.Default.GetHashCode(Id);
        }
    }
}
