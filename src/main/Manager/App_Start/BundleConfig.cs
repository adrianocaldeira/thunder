using System.Web.Optimization;

[assembly: WebActivator.PostApplicationStartMethod(typeof(Manager.App_Start.BundleConfig), "Register")]
namespace Manager.App_Start
{
    public class BundleConfig
    {
        public static void Register()
        {
            BundleTable.Bundles.Add(new ScriptBundle("~/scripts/jquery").Include(
                        "~/Scripts/jquery-1.*"));

            BundleTable.Bundles.Add(new ScriptBundle("~/scripts/jqueryui").Include(
                        "~/Scripts/jquery-ui*"));

            BundleTable.Bundles.Add(new ScriptBundle("~/scripts/manager").Include(
                        "~/Scripts/jquery.thunder-*",
                        "~/Scripts/jquery.maskMoney.js",
                        "~/Scripts/jquery.maskedinput-*",
                        "~/Scripts/manager.js"));

            BundleTable.Bundles.Add(new StyleBundle("~/content/manager/css").Include(
                "~/Content/manager/reset.css",
                "~/Content/manager/style.css",
                "~/Content/manager/color.css",
                "~/Content/manager/invalid.css"));

            BundleTable.Bundles.Add(new StyleBundle("~/content/manager/modal").Include(
                "~/Content/manager/reset.css",
                "~/Content/manager/style.css",
                "~/Content/manager/color.css",
                "~/Content/manager/invalid.css",
                "~/Content/manager/modal.css"));

            BundleTable.Bundles.Add(new StyleBundle("~/content/manager/login").Include(
                "~/Content/manager/reset.css",
                "~/Content/manager/style.css",
                "~/Content/manager/color.css",
                "~/Content/manager/invalid.css",
                "~/Content/manager/login.css"));

            BundleTable.Bundles.Add(new StyleBundle("~/content/jquerythunder/css").Include(
                  "~/Content/jquerythunder/jquery.thunder-*"));

            BundleTable.Bundles.Add(new StyleBundle("~/content/themes/base/css").Include(
                "~/Content/themes/base/jquery.ui.core.css",
                "~/Content/themes/base/jquery.ui.resizable.css",
                "~/Content/themes/base/jquery.ui.selectable.css",
                "~/Content/themes/base/jquery.ui.accordion.css",
                "~/Content/themes/base/jquery.ui.autocomplete.css",
                "~/Content/themes/base/jquery.ui.button.css",
                "~/Content/themes/base/jquery.ui.dialog.css",
                "~/Content/themes/base/jquery.ui.slider.css",
                "~/Content/themes/base/jquery.ui.tabs.css",
                "~/Content/themes/base/jquery.ui.datepicker.css",
                "~/Content/themes/base/jquery.ui.progressbar.css",
                "~/Content/themes/base/jquery.ui.theme.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}