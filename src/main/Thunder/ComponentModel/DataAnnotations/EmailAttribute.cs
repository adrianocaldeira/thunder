using System;
using System.ComponentModel.DataAnnotations;

namespace Thunder.ComponentModel.DataAnnotations
{
    ///<summary>
    /// E-mail validador
    ///</summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class EmailAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// Initialize new instance of <see cref="EmailAttribute"/>.
        /// </summary>
        public EmailAttribute() : base(@"^\w+([-+.]*[\w-]+)*@([\w-]+\.)+\w{2,}$")
        {
        }
    }
}
