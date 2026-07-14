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
            if (valueProviderResult == null) return null;

            var modelState = new ModelState { Value = valueProviderResult };
            modelBindingContext.ModelState.Add(modelBindingContext.ModelName, modelState);

            var attemptedValue = valueProviderResult.AttemptedValue;
            if (string.IsNullOrEmpty(attemptedValue)) return null;

            if (decimal.TryParse(attemptedValue, NumberStyles.Number, CultureInfo.CurrentCulture, out var value))
                return value;

            modelState.Errors.Add(string.Format("O valor '{0}' não é um número decimal válido.", attemptedValue));
            return null;
        }
    }
}
