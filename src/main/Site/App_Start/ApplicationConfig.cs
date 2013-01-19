using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Site.App_Start;
using Thunder.Web.Mvc;
using log4net.Config;

[assembly: WebActivator.PostApplicationStartMethod(typeof(ApplicationConfig), "Register")]
namespace Site.App_Start
{
    public static class ApplicationConfig
    {
        public static void Register()
        {
            XmlConfigurator.Configure();

            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());

            DataAnnotationsModelValidatorProvider.RegisterAdapterFactory(typeof(RequiredAttribute), 
                    (metadata, controllerContext, attribute) => new Adapters.RequiredAttributeAdapter(metadata, controllerContext, (RequiredAttribute)attribute));

            DefaultModelBinder.ResourceClassKey = "Messages";
            ValidationExtensions.ResourceClassKey = "Messages";
        }
    }
}