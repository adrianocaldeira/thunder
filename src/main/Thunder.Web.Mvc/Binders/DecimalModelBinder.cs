using System;
using System.Globalization;
using System.Web.Mvc;

namespace Thunder.Web.Mvc.Binders
{
    /// <summary>
    /// Decimal model binder
    /// </summary>
    public class DecimalModelBinder : IModelBinder
    {
        /// <summary>
        /// Bind model
        /// </summary>
        /// <param name="controllerContext"><see cref="ControllerContext"/></param>
        /// <param name="modelBindingContext"><see cref="ModelBindingContext"/></param>
        /// <returns>Object</returns>
        public object BindModel(ControllerContext controllerContext, ModelBindingContext modelBindingContext)
        {
            var valueProviderResult = modelBindingContext.ValueProvider.GetValue(modelBindingContext.ModelName);
            var modelState = new ModelState { Value = valueProviderResult };
            object actualValue = null;
            try
            {
                if (!string.IsNullOrEmpty(valueProviderResult.AttemptedValue))
                {
                    actualValue = Convert.ToDecimal(valueProviderResult.AttemptedValue, CultureInfo.CurrentCulture); 
                }
            }
            catch (FormatException e)
            {
                modelState.Errors.Add(e);
            }

            modelBindingContext.ModelState.Add(modelBindingContext.ModelName, modelState);

            return actualValue;
        }
    }
}
