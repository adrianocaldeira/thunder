using System;
using System.ComponentModel.DataAnnotations;

namespace Thunder.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Require if attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class RequiredIfAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessageFormatString = "The {0} field is required.";

        private readonly RequiredIfOperator _dependentPropertyComparison;
        private readonly string _dependentPropertyName;
        private readonly object _dependentPropertyValue;


        /// <summary>
        /// Initialize new instance of <see cref="RequiredIfOperator"/>.
        /// </summary>
        /// <param name="dependentPropertyName">Dependent property name</param>
        /// <param name="dependentPropertyComparison">Dependent property comparison</param>
        /// <param name="dependentPropertyValue">Dependent property value</param>
        public RequiredIfAttribute(string dependentPropertyName, RequiredIfOperator dependentPropertyComparison,
                                   object dependentPropertyValue)
        {
            _dependentPropertyName = dependentPropertyName;
            _dependentPropertyComparison = dependentPropertyComparison;
            _dependentPropertyValue = dependentPropertyValue;

            ErrorMessage = DefaultErrorMessageFormatString;
        }

        private bool ValidateDependentProperty(object actualPropertyValue)
        {
            switch (_dependentPropertyComparison)
            {
                case RequiredIfOperator.NotEqualTo:
                    return actualPropertyValue == null ? _dependentPropertyValue != null : !actualPropertyValue.Equals(_dependentPropertyValue);
                default:
                    return actualPropertyValue == null ? _dependentPropertyValue == null : actualPropertyValue.Equals(_dependentPropertyValue);
            }
        }

        /// <summary>
        /// Is valid
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                var dependentProperty = validationContext.ObjectInstance.GetType().GetProperty(_dependentPropertyName);
                var dependentPropertyValue = dependentProperty.GetValue(validationContext.ObjectInstance, null);

                if (ValidateDependentProperty(dependentPropertyValue))
                {
                    return new ValidationResult(string.Format(ErrorMessageString, validationContext.DisplayName));
                }
            }
            return ValidationResult.Success;
        }
    }
}