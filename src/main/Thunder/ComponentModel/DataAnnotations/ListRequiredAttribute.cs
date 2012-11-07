using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Thunder.ComponentModel.DataAnnotations
{
    /// <summary>
    /// List required attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class ListRequiredAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initialize new instance of <see cref="ListRequiredAttribute"/>.
        /// </summary>
        public ListRequiredAttribute(): base("{0} must have at least one element.")
        {
        }

        /// <summary>
        /// Value is valid
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Valid</returns>
        public override bool IsValid(object value)
        {
            var list = value as IList;
            return (list != null && list.Count > 0);
        }

        /// <summary>
        /// Format error message
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>Error formated</returns>
        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name);
        }
    }
}
