using System.Web.Mvc;

namespace ChuanglitouP2P.Areas.Topic
{
    public class TopicAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Topic";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            
            context.MapRoute(
                "20161111",
                "Topic/T20161111/Index.html",
                new { Controller = "T20161111", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "ChuanglitouP2P.Areas.Topic.Controllers" }
            );
            context.MapRoute(
                "Topic_default",
                "Topic/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "zhuanpan",
                "zhuanpan.html",
                //new { Controller = "T20160901", action = "Index", id = UrlParameter.Optional },
                new { Controller = "T20161212", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "ChuanglitouP2P.Areas.Topic.Controllers" }
            );
            context.MapRoute(
               "zhuolu",
               "zhuolu.html",
               new { Controller = "T20160906", action = "Index", id = UrlParameter.Optional },
               namespaces: new string[] { "ChuanglitouP2P.Areas.Topic.Controllers" }
           );
            context.MapRoute(
               "grabiPhone",
               "20160910.html",
               new { Controller = "T20160910", action = "Index", id = UrlParameter.Optional },
               namespaces: new string[] { "ChuanglitouP2P.Areas.Topic.Controllers" }
           );
            context.MapRoute(
               "act0715",
               "act0715.html",
               new { Controller = "T20160715", action = "Index", id = UrlParameter.Optional },
               namespaces: new string[] { "ChuanglitouP2P.Areas.Topic.Controllers" }
           );
            context.MapRoute(
               "yunying",
               "yunying.html",
               new { Controller = "T2016091401", action = "Index", id = UrlParameter.Optional },
               namespaces: new string[] { "ChuanglitouP2P.Areas.Topic.Controllers" }
           );
            context.MapRoute(
               "tuiguang",
               "tuiguang.html",
               new { Controller = "T20160819", action = "Index", id = UrlParameter.Optional },
               namespaces: new string[] { "ChuanglitouP2P.Areas.Topic.Controllers" }
           );
            context.MapRoute(
               "newhands",
               "20160901.html",
               new { Controller = "T20161212", action = "Index", id = UrlParameter.Optional },
               namespaces: new string[] { "ChuanglitouP2P.Areas.Topic.Controllers" }
           );
            context.MapRoute(
               "20161001",
               "20161001.html",
               new { Controller = "T20161001", action = "Index", id = UrlParameter.Optional },
               namespaces: new string[] { "ChuanglitouP2P.Areas.Topic.Controllers" }
           );
            context.MapRoute(
                "20161018",
                "20161018.html",
                namespaces: new string[] { "ChuanglitouP2P.Areas.Topic.Controllers" }
            );
            context.MapRoute(
               "20161110",
               "20161110.html",
               new { Controller = "T20161110", action = "Index", id = UrlParameter.Optional },
               namespaces: new string[] { "ChuanglitouP2P.Areas.Topic.Controllers" }
           );
            context.MapRoute(
              "20161122",
              "20161122.html",
              new { Controller = "T20161122", action = "Index", id = UrlParameter.Optional },
              namespaces: new string[] { "ChuanglitouP2P.Areas.Topic.Controllers" }
          );
            context.MapRoute(
             "20170103",
             "20170103.html",
             new { Controller = "T20170103", action = "Index", id = UrlParameter.Optional },
             namespaces: new string[] { "ChuanglitouP2P.Areas.Topic.Controllers" }
         );
            context.MapRoute(
              "20170104",
              "20170104.html",
              new { Controller = "T20170104", action = "Index", id = UrlParameter.Optional },
              namespaces: new string[] { "ChuanglitouP2P.Areas.Topic.Controllers" }
          );
            context.MapRoute(
             "20170106",
             "20170106.html",
             new { Controller = "T20170106", action = "Index", id = UrlParameter.Optional },
             namespaces: new string[] { "ChuanglitouP2P.Areas.Topic.Controllers" }
         );
        }
    }
}