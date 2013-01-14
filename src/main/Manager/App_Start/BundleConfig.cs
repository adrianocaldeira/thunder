using System.Web.Optimization;

namespace Manager.App_Start
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/scripts/jquery").Include(
                        "~/Scripts/jquery-1.*"));

            bundles.Add(new ScriptBundle("~/scripts/jqueryui").Include(
                        "~/Scripts/jquery-ui*"));

            bundles.Add(new ScriptBundle("~/scripts/manager").Include(
                        "~/Scripts/jquery.thunder-*",
                        "~/Scripts/jquery.maskMoney.js",
                        "~/Scripts/jquery.maskedinput-*",
                        "~/Scripts/jquery.uploadify-*",
                        "~/Scripts/manager.js"));

            bundles.Add(new StyleBundle("~/content/manager/css").Include(
                "~/Content/manager/reset.css",
                "~/Content/manager/style.css",
                "~/Content/manager/green.css",
                "~/Content/manager/invalid.css"));

            bundles.Add(new StyleBundle("~/content/manager/login").Include(
                "~/Content/manager/reset.css",
                "~/Content/manager/style.css",
                "~/Content/manager/green.css",
                "~/Content/manager/invalid.css",
                "~/Content/manager/login.css"));

            bundles.Add(new StyleBundle("~/content/jquerythunder/css").Include(
                  "~/Content/jquerythunder/jquery.thunder-*"));

            bundles.Add(new StyleBundle("~/content/themes/base/css").Include(
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

            bundles.Add(new StyleBundle("~/content/uploadify/css").Include(
                  "~/Content/uploadify/uploadify.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}