using System.Web;
using System.Web.Optimization;

namespace Fullbar.Olympus.MVC
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.11.js",
                        "~/Scripts/jquery.mask.js"
                        //"~/Scripts/jquery-ui-1.8.24.js"

                        //"~/Scripts/jspdf.js",
                        //"~/Scripts/jspdf.plugin.addimage.js"

                        ));


            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.unobtrusive*",
            //            "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css", "~/Content/themes/base/jquery-ui.css"));

            BundleTable.EnableOptimizations = false;
        }
    }
}