using System.Web;
using System.Web.Optimization;

namespace NBUProject
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/kendoStyles").Include(
                          "~/Content/kendo/kendo.common.min.css",
                          "~/Content/kendo/kendo.default.min.css"));


            bundles.Add(new ScriptBundle("~/bundles/jqueryKendo").Include(
                     "~/Scripts/kendo/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
                     "~/Scripts/kendo/kendo.web.min.js",
                     // "~/Scripts/kendo/kendo.timezones.min.js", // uncomment if using the Scheduler
                     "~/Scripts/kendo/kendo.aspnetmvc.min.js"));  //after jquery


            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryunob").Include(
                        "~/Scripts/jquery.unobtrusive*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/site.css",
                      "~/Content/bootstrap.min.css"));

            bundles.IgnoreList.Clear();
        }
    }
}
