using System.Web;
using System.Web.Optimization;

namespace DocTrack
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Assets/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Assets/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryvalunobtrusive").Include(
            "~/Assets/Scripts/jquery.validate.unobtrusive*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Assets/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Assets/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Assets/Content/bootstrap.css",
                      "~/Assets/Content/site.css"));

            bundles.Add(new StyleBundle("~/keen/css").Include(
                    "~/Assets/Theme/assets/plugins/global/plugins.bundle.css",
                    "~/Assets/Theme/assets/css/style.bundle.css",
                    "~/Assets/Theme/assets/css/skins/header/base/light.css",
                    "~/Assets/Theme/assets/css/skins/header/menu/light.css",
                    "~/Assets/Theme/assets/css/skins/brand/navy.css",
                    "~/Assets/Theme/assets/css/skins/aside/navy.css"
                ));

            bundles.Add(new StyleBundle("~/plugins/style").Include(
                    "~/Assets/Theme/vendor/select2-bootstrap4-theme/select2-bootstrap4.css",
                    "~/Assets/Theme/assets/plugins/custom/datatables/datatables.bundle.css",
                    "~/Assets/Theme/vendor/select2/css/select2.css"
                ));

            bundles.Add(new ScriptBundle("~/plugins/js").Include(
                    "~/Assets/Theme/assets/plugins/custom/datatables/datatables.bundle.js",
                    "~/Assets/Theme/vendor/select2/js/select2.js",
                    "~/Assets/Theme/vendor/sweetalert2/dist/sweetalert2.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/ajax.functions").Include(
                    "~/Assets/Scripts/ajax.functions.js"
                ));

            bundles.Add(new ScriptBundle("~/keen/js").Include(
                    "~/Assets/Theme/assets/plugins/global/plugins.bundle.js",
                    "~/Assets/Theme/assets/js/scripts.bundle.js"
                ));
        }
    }
}
