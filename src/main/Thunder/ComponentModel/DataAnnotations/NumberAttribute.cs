using System;
using System.ComponentModel.DataAnnotations;

namespace Thunder.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Positive number validator
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class NumberAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initialize new instance of <see cref="NumberAttribute"/>.
        /// </summary>
        public NumberAttribute()
        {
            Maximum = 999;
        }

        /// <summary>
        /// Get or set minimum mumber. Default 0.
        /// </summary>
        public int Minimum { get; set; }

        /// <summary>
        /// Get or set maximum mumber. Default 999.
        /// </summary>
        public int Maximum { get; set; }

        /// <summary>
        /// Is number
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            if (value == null)
                return true;

            int number;

            if (int.TryParse(value.ToString(), out number))
            {
                return (number >= Minimum && number <= Maximum);
            }

            return false;
        }
    }
}
