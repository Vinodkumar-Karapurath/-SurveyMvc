using System.Web;
using System.Web.Optimization;

namespace SurveyMvc
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/Chart.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui")
            .Include("~/Scripts/jquery-ui-{version}.js").Include(
                   "~/Scripts/jquery-ui-1.11.4.min"));

            bundles.Add(new StyleBundle("~/Content/csss")
                                .Include("~/Content/bootstrap.css")
                                .Include("~/Content/side.css")
                                .Include("~/Content/Site.css")
                                .Include("~/Content/css/font-awesome.min.css")
                                .Include( "~/Content/css/font-awesome-ie7.min.css")
                                .Include("~/Content/css/halflings.css"));

            bundles.Add(new StyleBundle("~/Content/css/font")
                               .Include("~/Content/css/font-awesome.min.css")
                               .Include("~/Content/css/font-awesome-ie7.min.css")
                               .Include("~/Content/css/halflings.css"));

            
            bundles.Add(new StyleBundle("~/Content/jqueryui")
               .Include("~/Content/themes/base/all.css")
               .Include("~/Content/themes/base/theme.css")
               .Include("~/Content/themes/base/datepicker.css"));
        }
    }
}
