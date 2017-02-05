using System.Web;
using System.Web.Optimization;

namespace RaceAnalysis
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            
               bundles.Add(new StyleBundle("~/bundles/themes/css").Include(
              "~/Content/themes/base/ui.core.css",
              "~/Content/themes/base/ui.resizable.css",
              "~/Content/themes/base/ui.selectable.css",
              "~/Content/themes/base/ui.accordion.css",
              "~/Content/themes/base/ui.autocomplete.css",
              "~/Content/themes/base/ui.button.css",
              "~/Content/themes/base/dialog.css",
              "~/Content/themes/base/slider.css",
              "~/Content/themes/base/ui.tabs.css",
              "~/Content/themes/base/datepicker.css",
              "~/Content/themes/base/progressbar.css",
              "~/Content/themes/base/theme.css",
              "~/Content/themes/base/jquery-ui.css"));

   
          bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui.js",
                        "~/Scripts/jquery-ui-{version}.js",
                        "~/Scripts/jquery-ui-touch-punch-min.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                         "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.unobtrusive.js"

                        ));


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/site").Include(
                      "~/Scripts/site.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/select").Include(
                      "~/Scripts/jquery.sumoselect.min.js",
                      "~/Scripts/select2.min.js"
                ));

            bundles.Add(new StyleBundle("~/bundles/css").Include(
            "~/Content/bootstrap.css",
            "~/Content/sumoselect.css",
            "~/Content/select2.min.css",
            "~/Content/site.css",
            "~/Content/sitemenu.css"
            ));

    
    


        }
    }
}
