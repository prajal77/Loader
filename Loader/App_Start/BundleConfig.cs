﻿using System.Web;
using System.Web.Optimization;

namespace Loader
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        //"~/Scripts/jquery-3.6.0.min.js",
                        "~/Scripts/jquery-3.7.1.min.js",
                        //"~/Scripts/jquery-{version}.js",

                        "~/Scripts/jquery-message-box.js", "~/Scripts/ch-dialog.js"
                        //"~/Scripts/jquery.js"
                        ));

            

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      //"~/Scripts/popper.js",
                      "~/Scripts/bootstrap.js",
                      //"~/Scripts/bootstrap.bundle.min.js",
                      "~/Scripts/respond.js",
                      "~/AdminLTE/dist/js/app.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                  "~/Scripts/ch-dpicker.js"
                ));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/font-awesome.css",
          
              "~/AdminLTE/dist/css/AdminLTE.css",
                      "~/bootstrap/css/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/AdminLTE/css").Include(
                           "~/Content/messagebox.css ",
                    "~/Content/ch-dialog.css",
                    "~/AdminLTE/dist/css/AdminLTE.css",
                    "~/bootstrap/css/bootstrap.css",
                    "~/Content/font-awesome.min.css",
                    "~/AdminLTE/dist/css/skins/_all-skins.min.css",        
                    "~/AdminLTE/plugins/iCheck/flat/blue.css"
             
                ));
            bundles.Add(new ScriptBundle("~/bundles/select2").Include(
                 "~/Scripts/select2.min.js"
             ));

            bundles.Add(new StyleBundle("~/Content/select2").Include(
                "~/Content/select2.min.css"
            ));
        }
    }
}
