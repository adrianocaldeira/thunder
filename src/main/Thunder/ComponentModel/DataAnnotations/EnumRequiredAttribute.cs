using System;
using System.ComponentModel.DataAnnotations;

namespace Thunder.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Enum required attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class EnumRequiredAttribute : RequiredAttribute
    {
        /// <summary>
        /// Initialize new instance of <see cref="EnumRequiredAttribute"/>.
        /// </summary>
        public EnumRequiredAttribute()
            : this("NotSet")
        {
        }

        /// <summary>
        /// Initialize new instance of <see cref="EnumRequiredAttribute"/>.
        /// </summary>
        /// <param name="notSetValue">Not set value</param>
        public EnumRequiredAttribute(string notSetValue)
        {
            if (String.IsNullOrWhiteSpace(notSetValue))
            {
                throw new ArgumentNullException("notSetValue");
            }

            NotSetValue = notSetValue;
        }

        /// <summary>
        /// Get or set not set value
        /// </summary>
        public string NotSetValue { get; set; }
        
        /// <summary>
        /// Is valid
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            var notSet = Enum.Parse(value.GetType(), NotSetValue);
            
            return !Equals(value, notSet);
        }
    }
}