﻿using System.Web;
using System.Web.Optimization;

namespace ESRGC.Broadband.ETL.CensusBlock
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles) {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jqueryPlugins/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        //"~/Scripts/jqueryPlugins/jquery.unobtrusive*",
                        "~/Scripts/jqueryPlugins/jquery.validate.js"));
            
            //App base Bundle
            bundles.Add(new ScriptBundle("~/bundles/appBase").Include(
                "~/Scripts/ESRGC/Script.js",
                "~/Scripts/ESRGC/component.js",
                "~/Scripts/ESRGC/Application/application.js",
                "~/Scripts/ESRGC/Controller/base.js",
                "~/Scripts/ESRGC/Model/base.js",
                "~/Scripts/ESRGC/Store/base.js"
            ));
            //upload app bundle
            bundles.Add(new ScriptBundle("~/bundles/uploadApp").Include(
                "~/Upload/app.js",
                "~/Upload/app/controller/upload.js"
            ));
            //data map app bundle
            bundles.Add(new ScriptBundle("~/bundles/mapDataApp").Include(
                "~/MapData/app.js",
                "~/MapData/app/controller/mapData.js",
                "~/MapData/app/view/mapData.js"
            ));
            //preview mapping app bundle
            bundles.Add(new ScriptBundle("~/bundles/commitDataApp").Include(
                    "~/CommitData/app.js",
                    "~/CommitData/app/controller/commitData.js",
                    "~/CommitData/app/store/commitData.js",
                    "~/CommitData/app/store/commitProgress.js"

            ));
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/customCSS/style").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
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

            bundles.Add(new StyleBundle("~/Content/bootstrap/css/style").Include(
                "~/Content/bootstrap/css/bootstrap.css",
                "~/Content/bootstrap/css/bootstrap-responsive.css"));
            //BundleTable.EnableOptimizations = true;
        }
    }
}