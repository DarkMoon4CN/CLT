using System.Web.Mvc;

namespace WeiXin.Areas.activity
{
    public class activityAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "activity";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "20161111",
                "activity/W20161111/Index.html",
                new { Controller = "W20161111", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "WeiXin.Areas.activity.Controllers" }
            );
            context.MapRoute(
                "activity_default",
                "activity/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "zhuolu",
                "zhuolu.html",
                new { Controller = "W20160906", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "WeiXin.Areas.activity.Controllers" }
            );
            context.MapRoute(
                "20160901",
                "20160901.html",
                new { Controller = "W20161212", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "WeiXin.Areas.activity.Controllers" }
            );
            context.MapRoute(
                "zhuanpan",
                "zhuanpan.html",
                new { Controller = "W201609011", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "WeiXin.Areas.activity.Controllers" }
            );
            context.MapRoute(
                "act0715",
                "act0715.html",
                new { Controller = "W20160715", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "WeiXin.Areas.activity.Controllers" }
            );
            context.MapRoute(
                "20160910",
                "20160910.html",
                new { Controller = "W20160910", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "WeiXin.Areas.activity.Controllers" }
            );
            context.MapRoute(
                "register",
                "register.html",
                new { Controller = "register", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "WeiXin.Controllers" }
            );
            context.MapRoute(
                "20161001",
                "20161001.html",
                new { Controller = "W20161001", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "WeiXin.Areas.activity.Controllers" }
            );
            context.MapRoute(
                "security",
                "security.html",
                new { Controller = "Security", action = "security", id = UrlParameter.Optional },
                namespaces: new string[] { "WeiXin.Controllers" }
            );
            context.MapRoute(
                "Contactus",
                "Contactus.html",
                new { Controller = "Aboutme", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "WeiXin.Controllers" }
            );
            context.MapRoute(
                "login",
                "login.html",
                new { Controller = "Login", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "WeiXin.Controllers" }
            );
            context.MapRoute(
                "20160926",
                "20160926.html",
                new { Controller = "W20160926", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "WeiXin.Areas.activity.Controllers" }
            );
            context.MapRoute(
               "flow",
               "flow.html",
               new { Controller = "WLiuMi", action = "Index", id = UrlParameter.Optional },
               namespaces: new string[] { "WeiXin.Areas.activity.Controllers" }
           );
            context.MapRoute(
               "receiveFlow",
               "receiveFlow.html",
               new { Controller = "WLiuMi", action = "Receive", id = UrlParameter.Optional },
               namespaces: new string[] { "WeiXin.Areas.activity.Controllers" }
           );
            context.MapRoute(
              "20161018",
              "20161018.html",
              new { Controller = "W20161018", action = "Index", id = UrlParameter.Optional },
              namespaces: new string[] { "WeiXin.Areas.activity.Controllers" }
          );

            context.MapRoute(
             "20161110",
             "20161110.html",
             new { Controller = "W20161110", action = "Index", id = UrlParameter.Optional },
             namespaces: new string[] { "WeiXin.Areas.activity.Controllers" }
         );
            context.MapRoute(
              "20161122",
              "20161122.html",
              new { Controller = "W20161122", action = "Index", id = UrlParameter.Optional },
              namespaces: new string[] { "WeiXin.Areas.activity.Controllers" }
            );
            context.MapRoute(
             "20170103",
             "20170103.html",
             new { Controller = "W20170103", action = "Index", id = UrlParameter.Optional },
             namespaces: new string[] { "WeiXin.Areas.activity.Controllers" }
           );
            context.MapRoute(
              "20170104",
              "20170104.html",
              new { Controller = "W20170104", action = "Index", id = UrlParameter.Optional },
              namespaces: new string[] { "WeiXin.Areas.activity.Controllers" }
            );
            context.MapRoute(
             "20170106",
             "20170106.html",
             new { Controller = "W20170106", action = "Index", id = UrlParameter.Optional },
             namespaces: new string[] { "WeiXin.Areas.activity.Controllers" }
           );
        }
    }
}