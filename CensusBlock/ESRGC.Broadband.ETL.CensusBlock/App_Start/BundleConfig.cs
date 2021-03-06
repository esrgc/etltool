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
            //bootstrap bundles
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap/bootstrap*"
            ));
            //App base Bundle
            bundles.Add(new ScriptBundle("~/bundles/appBase").Include(
                "~/Scripts/ESRGC/Script.js",
                "~/Scripts/ESRGC/component.js",
                "~/Scripts/ESRGC/Application/application.js",
                "~/Scripts/ESRGC/Controller/base.js",
                "~/Scripts/ESRGC/Model/base.js",
                "~/Scripts/ESRGC/Store/base.js"
            ));
            //security app bundle
            //bundles.Add(new ScriptBundle("~/bundles/securityApp").Include(
            //    "~/Scripts/Applications/Home/app.js",
            //    "~/Scripts/Applications/Home/app/controller/security.js",
            //    "~/Scripts/Applications/Home/app/store/login.js"
            //));
            //upload app bundle
            bundles.Add(new ScriptBundle("~/bundles/uploadApp").Include(
                "~/Scripts/Applications/Upload/app.js",
                "~/Scripts/Applications/Upload/app/controller/upload.js"
            ));
            //data map app bundle
            bundles.Add(new ScriptBundle("~/bundles/mapDataApp").Include(
                "~/Scripts/Applications/MapData/app.js",
                "~/Scripts/Applications/MapData/app/controller/mapData.js",
                "~/Scripts/Applications/MapData/app/view/mapData.js"
            ));
            //preview mapping app bundle
            bundles.Add(new ScriptBundle("~/bundles/commitDataApp").Include(
                "~/Scripts/Applications/CommitData/app.js",
                "~/Scripts/Applications/CommitData/app/controller/commitData.js",
                "~/Scripts/Applications/CommitData/app/store/commitData.js",
                "~/Scripts/Applications/CommitData/app/store/commitProgress.js"

            ));
            bundles.Add(new ScriptBundle("~/bundles/dashboardApp").Include(
                "~/Scripts/Applications/Dashboard/app.js",
                "~/Scripts/Applications/Dashboard/app/controller/dashboard.js",
                "~/Scripts/Applications/Dashboard/app/store/submission.js"
            ));
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/siteStyle").Include(
                "~/Content/site.css",
                "~/Content/stickyFooter.css"
            ));
                        
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
                "~/Content/themes/base/jquery.ui.theme.css"
            ));

            bundles.Add(new StyleBundle("~/Content/bootstrap/css/style").Include(
                "~/Content/bootstrap/css/bootstrap.css",
                "~/Content/bootstrap/css/bootstrap-responsive.css",
                "~/Content/bootstrap/css/datepicker.css"));
            //BundleTable.EnableOptimizations = true;
        }
    }
}