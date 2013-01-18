using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Resources;

namespace $rootnamespace$.Adapters
{
    public class RequiredAttributeAdapter : System.Web.Mvc.RequiredAttributeAdapter
    {
        public RequiredAttributeAdapter(ModelMetadata metadata, ControllerContext context, RequiredAttribute attribute)
            : base(metadata, context, attribute)
        {
            if (!string.IsNullOrWhiteSpace(Attribute.ErrorMessage)) return;

            if (Attribute.ErrorMessageResourceType == null)
            {
                Attribute.ErrorMessageResourceType = typeof (Message);
            }

            if (string.IsNullOrWhiteSpace(Attribute.ErrorMessageResourceName))
            {
                Attribute.ErrorMessageResourceName = "PropertyValueRequired";
            }
        }
    }
}