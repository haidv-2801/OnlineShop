using System.Web;
using System.Web.Optimization;

namespace OnlineShop
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       
                        "~/Assets/Client/js/old-jquery/respond.min.js",
                        "~/Assets/Client/js/old-jquery/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/jscore").Include(
                        "~/Assets/Client/js/jquery-1.11.3.min.js",
                        "~/Assets/Client/js/jquery-ui.js",
                        "~/Assets/Client/js/bootstrap.min.js",
                        "~/Assets/Client/js/move-top.js",
                        "~/Assets/Client/js/easing.js",
                        "~/Assets/Client/js/startstop-slider.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/controller").Include(
                        "~/Assets/Client/js/controller/baseController.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      
                      "~/Assets/Client/js/old-jquery/respond.js",
                      "~/Assets/Client/js/old-jquery/jquery.timeago.js"));

            bundles.Add(new StyleBundle("~/bundles/core").Include(
                      "~/Assets/Client/css/style.css",
                      "~/Assets/Client/css/slider.css",
                      "~/Assets/Client/css/font-awesome.min.css",
                      "~/Assets/Client/css/jquery-ui.css",
                      "~/Assets/Client/css/bootstrap.css",
                      "~/Assets/Client/css/bootstrap-theme.css"

                      ));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/PagedList.css"
                ));

            bundles.Add(new StyleBundle("~/Content/cssComment").Include(
                      "~/Content/comment.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
