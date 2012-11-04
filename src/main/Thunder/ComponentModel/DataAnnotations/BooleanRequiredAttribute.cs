using System;
using System.ComponentModel.DataAnnotations;

namespace Thunder.ComponentModel.DataAnnotations
{
    ///<summary>
    /// Boolean validator
    ///</summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class BooleanRequiredAttribute : ValidationAttribute
    {
        /// <summary>
        /// Valid object
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            if(value is bool)
            {
                return true;
            }

            bool result;

            return bool.TryParse(value.ToString(), out result);
        }
    }
}
