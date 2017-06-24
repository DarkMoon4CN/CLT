using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WeiXin
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
            name: "indexhome",
            url: "index.html",
            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

           routes.MapRoute(
           name: "DownLoad",
           url: "DownLoad.html",
           defaults: new { controller = "Home", action = "DownLoad", id = UrlParameter.Optional }
           );

           routes.MapRoute(
           name: "App",
           url: "App.html",
           defaults: new { controller = "Home", action = "App", id = UrlParameter.Optional }
           );
            routes.MapRoute(
           name: "BigData",
           url: "BigData.html",
           defaults: new { controller = "BigData", action = "Index", id = UrlParameter.Optional }
             );

            routes.MapRoute(
           name: "get_p2p",
           url: "get_p2p",
           defaults: new { controller = "XiCaiChannel", action = "get_p2p", id = UrlParameter.Optional }
            );
            routes.MapRoute(
             name: "AutoRegisterLogin",
             url: "AutoRegisterLogin",
             defaults: new { controller = "XiCaiChannel", action = "AutoRegisterLogin", id = UrlParameter.Optional }
              );
            routes.MapRoute(
             name: "Tongji_User",
             url: "Tongji_User",
             defaults: new { controller = "XiCaiChannel", action = "Tongji_User", id = UrlParameter.Optional }
              );
            routes.MapRoute(
             name: "Tongji_Invest",
             url: "Tongji_Invest",
             defaults: new { controller = "XiCaiChannel", action = "Tongji_Invest", id = UrlParameter.Optional }
              );
            routes.MapRoute(
             name: "invest_borrow",
             url: "invest_borrow_{id}.html",
             defaults: new { controller = "Home", action = "ProjectDetail", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                "checkregister",
                "checkregister.html",
                new { Controller = "register", action = "checkmobile", id = UrlParameter.Optional },
                namespaces: new string[] { "WeiXin.Controllers" }
            );
            routes.MapPageRoute(//一元抢手机，下线了
                "20160914", "20160914.html", "~/Areas/activity/views/W20160914/index.aspx", false, 
                new RouteValueDictionary { { "controller", "*" }, { "action", "*" } });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

           
        }
    }
}
