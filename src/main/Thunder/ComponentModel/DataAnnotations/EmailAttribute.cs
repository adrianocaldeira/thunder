using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Thunder.ComponentModel.DataAnnotations
{
    ///<summary>
    /// E-mail validador
    ///</summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class EmailDomainAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// Initialize new instance of <see cref="EmailDomainAttribute"/>.
        /// </summary>
        public EmailDomainAttribute() : base(@"^\w+([-+.]*[\w-]+)*@(\w+([-.]?\w+)){1,}\.\w{2,}$")
        {
        }

        /// <summary>Checks whether the value entered by the user matches the regular expression pattern.</summary>
        /// <param name="value">The data field value to validate.</param>
        /// <returns>
        /// <see langword="true" /> if validation is successful; otherwise, <see langword="false" />.</returns>
        /// <exception cref="T:System.ComponentModel.DataAnnotations.ValidationException">The data field value did not match the regular expression pattern.</exception>
        public override bool IsValid(object value)
        {
            return value == null || Regex.Matches(value.ToString(), @"\@", RegexOptions.Singleline).Count == 1;
        }
    }
}
