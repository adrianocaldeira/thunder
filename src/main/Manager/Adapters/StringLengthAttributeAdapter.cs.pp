using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Resources;

namespace $rootnamespace$.Adapters
{
    public class StringLengthAttributeAdapter : System.Web.Mvc.StringLengthAttributeAdapter
    {
        public StringLengthAttributeAdapter(ModelMetadata metadata, ControllerContext context, StringLengthAttribute attribute)
            : base(metadata, context, attribute)
        {
            if (!string.IsNullOrWhiteSpace(Attribute.ErrorMessage)) return;

            if (Attribute.ErrorMessageResourceType == null)
            {
                Attribute.ErrorMessageResourceType = typeof(Message);
            }

            if (string.IsNullOrWhiteSpace(Attribute.ErrorMessageResourceName))
            {
                Attribute.ErrorMessageResourceName = "PropertyValueStringLength";
            }
        }
    }
}