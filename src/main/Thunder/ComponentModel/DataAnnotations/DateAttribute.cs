using System;
using System.ComponentModel.DataAnnotations;

namespace Thunder.ComponentModel.DataAnnotations
{
    ///<summary>
    /// Date validador
    ///</summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class DateAttribute : ValidationAttribute
    {
        /// <summary>
        /// IsValid
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            try
            {
                if (value == null)
                {
                    return true;
                }

                if (value is string)
                {
                    return ((string) value).IsDate();
                }

                var date = (DateTime) value;

                return !date.Equals(DateTime.MinValue) && date.ToShortDateString().IsDate();
            }
            catch
            {
                return false;
            }
        }
    }
}