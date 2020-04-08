using System;
using System.ComponentModel.DataAnnotations;

namespace Thunder.ComponentModel.DataAnnotations
{
    ///<summary>
    /// E-mail validador
    ///</summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class EmailAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// Initialize new instance of <see cref="EmailAttribute"/>.
        /// </summary>
        public EmailAttribute() : base(@"^\w+([-+.]*[\w-]+)*@(\w+([-.]?\w+)){1,}\.\w{2}$")
        {
        }
    }
}
