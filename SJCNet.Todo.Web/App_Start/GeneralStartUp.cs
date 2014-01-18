using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

[assembly: WebActivator.PostApplicationStartMethod(typeof(SJCNet.Todo.Web.App_Start.GeneralStartUp), "PreStart")]

namespace SJCNet.Todo.Web.App_Start
{
    public class GeneralStartUp
    {
        public static void PreStart()
        {
            // Add your start logic here
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}