using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Resources;

namespace $rootnamespace$.Adapters
{
    public class RangeAttributeAdapter : System.Web.Mvc.RangeAttributeAdapter
    {
        public RangeAttributeAdapter(ModelMetadata metadata, ControllerContext context, RangeAttribute attribute) : base(metadata, context, attribute)
        {
            if (!string.IsNullOrWhiteSpace(Attribute.ErrorMessage)) return;

            if (Attribute.ErrorMessageResourceType == null)
            {
                Attribute.ErrorMessageResourceType = typeof(Message);
            }

            if (string.IsNullOrWhiteSpace(Attribute.ErrorMessageResourceName))
            {
                if (metadata.ModelType == typeof(short) || metadata.ModelType == typeof(int) || metadata.ModelType == typeof(long))
                {
                    Attribute.ErrorMessageResourceName = "PropertyValueRangeNumber";
                }
                else if (metadata.ModelType == typeof(decimal))
                {
                    Attribute.ErrorMessageResourceName = "PropertyValueRangeDecimal";
                }
                else
                {
                    Attribute.ErrorMessageResourceName = "PropertyValueRequired";    
                }
                
            }
        }
    }
}