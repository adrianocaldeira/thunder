using System;
using System.ComponentModel.DataAnnotations;

namespace Thunder.ComponentModel.DataAnnotations
{
    ///<summary>
    /// Currency validator
    ///</summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class CurrencyAttribute : ValidationAttribute
    {
        /// <summary>
        /// Accept negative
        /// </summary>
        public bool AcceptNegative { get; set; }

        /// <summary>
        /// Currency is valid
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return false;

            decimal currencyValue;
            decimal.TryParse(value.ToString(), out currencyValue);

            if (currencyValue == 0)
                return false;

            return AcceptNegative || currencyValue >= 0;
        }
    }
}
