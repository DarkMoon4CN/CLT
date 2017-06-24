using ChuanglitouP2P.Common;
using ChuanglitouP2P.Common.Task;
using StackExchange.Profiling;
using StackExchange.Profiling.EntityFramework6;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ChuanglitouP2P
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes); 
            MiniProfilerEF6.Initialize();

            BundleConfig.RegisterBundles(BundleTable.Bundles);



            TaskManager.Instance.CreateTask(typeof(CheckOrderTask), 0);
        }

        protected void Application_BeginRequest()
        {
            MiniProfiler.Start();


        }
        protected void Application_EndRequest()
        {
            MiniProfiler.Stop();
        }

    }
}
