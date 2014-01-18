using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace SJCNet.Todo.Web.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/vendor").Include(
                "~/scripts/jquery-{version}.js",
                "~/scripts/jquery-ui-{version}.js",
                "~/scripts/knockout-{version}.js",
                "~/scripts/sammy-{version}.js",
                "~/scripts/moment.js",
                "~/scripts/Q.js",
                "~/scripts/datajs-{version}.js",
                "~/scripts/breeze.debug.js",
                "~/scripts/toastr.js",
                "~/scripts/moment.js",
                "~/scripts/bootstrap.js",
                "~/scripts/bootstrap-datepicker.js"
            ));

            bundles.Add(new ScriptBundle("~/Scripts/app").Include(
                "~/scripts/custom/knockout.custom.js",
                "~/scripts/custom/custom.js"
            ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap/bootstrap.css",
                "~/Content/bootstrap/bootstrap-responsive.css",
                "~/Content/datepicker.css",
                "~/Content/durandal.css",
                "~/Content/font-awesome.css",
                "~/Content/toastr.css",
                 "~/Content/site.css"
            ));
        }
    }
}