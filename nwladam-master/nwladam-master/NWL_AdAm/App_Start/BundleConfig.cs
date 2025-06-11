using System.Web.Optimization;

namespace NWL_AdAm
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-3.6.0/jquery.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap4/bootstrap.bundle.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/Scripts/app/base/loadingoverlay.min.js",
                "~/Scripts/app/base/sweetalert2.min.js",
                "~/Scripts/app/base/jquery.twbsPagination.js",
                "~/Scripts/app/base-app.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/MajesticAdmin").IncludeDirectory(
                "~/Scripts/app/MajesticAdmin", "*.js", true));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/app/style.css",
                "~/Content/app/materialdesignicons.min.css",
                "~/Content/app/sweetalert2.min.css"
                ));
            
        }
    }
}
