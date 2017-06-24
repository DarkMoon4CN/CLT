using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ChuanglitouP2P
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
            name: "NewCashHome",
            url: "Cash",
            defaults: new { controller = "Cash", action = "NewCash", id = UrlParameter.Optional }
              );
            routes.MapRoute(
            name: "NewCash",
            url: "Cash/Index",
            defaults: new { controller = "Cash", action = "NewCash", id = UrlParameter.Optional }
              );
            routes.MapRoute(
            name: "Register",
            url: "Register.html",
            defaults: new { controller = "Register", action = "Index", id = UrlParameter.Optional }
              );
            routes.MapRoute(
            name: "BigData",
            url: "BigData.html",
            defaults: new { controller = "BigData", action = "Index", id = UrlParameter.Optional }
              );
            routes.MapRoute(
            name: "App",
            url: "App.html",
            defaults: new { controller = "Home", action = "App", id = UrlParameter.Optional }
            );

            routes.MapRoute(
             name: "WDLogin",
             url: "WDLogin",
             defaults: new { controller = "WangDai", action = "Login" }
              );
            routes.MapRoute(
             name: "WDGetData",
             url: "WDGetData",
             defaults: new { controller = "WangDai", action = "Index" }
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
             name: "Invitation",
             url: "Invitation/{id}.html",
             defaults: new { controller = "Invitation", action = "Index", id = UrlParameter.Optional }
         );

            routes.MapRoute(
                name: "Video",
                url: "Video.html",
                defaults: new { controller = "Home", action = "Video", id = UrlParameter.Optional },
                namespaces: new string[] { "ChuanglitouP2P.Controllers" }
            );

            routes.MapRoute(
                  name: "CLT_company",
                  url: "CLT_company.html",
                  defaults: new { controller = "about", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                 name: "CLT_security1",
                 url: "securityIndex.html",
                 defaults: new { controller = "CLT_security", action = "Index", id = UrlParameter.Optional },
                  namespaces: new string[] { "ChuanglitouP2P.Controllers" }
           );

            routes.MapRoute(
                 name: "Mediareports",
                 url: "CLT_Mediareports_{id}.html",
                 defaults: new { controller = "about", action = "Detail", id = UrlParameter.Optional },
                  namespaces: new string[] { "ChuanglitouP2P.Controllers" }
           );

            routes.MapRoute(
              name: "HelpDetail",
              url: "clt_Detail_{path1}_{id}.html",
              defaults: new { controller = "HelpCenter", action = "Index", id = UrlParameter.Optional, path1 = UrlParameter.Optional }
            );

            routes.MapRoute(
                 name: "HelpList",
                 url: "clt_{path1}.html",
                 defaults: new { controller = "HelpCenter", action = "HelpList", id = UrlParameter.Optional, path1 = UrlParameter.Optional }
             );
          //  routes.MapRoute(
          //    name: "indexhome",
          //    url: "index.html",
          //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
          //);

            routes.MapRoute(
                name: "Signin",
                url: "login.html",
                defaults: new { controller = "Signin", action = "Index", id = UrlParameter.Optional }
            );


            routes.MapRoute(
              name: "forget1",
              url: "forget.html",
              defaults: new { controller = "Signin", action = "forget", id = UrlParameter.Optional }
          );

            routes.MapRoute(
               name: "logout",
               url: "logout.html",
               defaults: new { controller = "logout", action = "Index", id = UrlParameter.Optional }
           );


            routes.MapRoute(
               name: "invest_borrow",
               url: "invest_borrow_{id}.html",
               defaults: new { controller = "invest_borrow", action = "Index", id = UrlParameter.Optional }
           );


            routes.MapRoute(
               name: "Investlist",
               url: "Investlist.html",
               defaults: new { controller = "Investlist", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                 name: "guide",
                 url: "guide.html",
                 defaults: new { controller = "guide", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                  name: "about",
                  url: "about.html",
                  defaults: new { controller = "about", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "Loans",
               url: "Loans.html",
               defaults: new { controller = "Loans", action = "Index", id = UrlParameter.Optional }
           );
            //验证码图片
            routes.MapRoute(
              name: "Validate",
              url: "Validate.html",
              defaults: new { controller = "Loans", action = "ImageValidate", id = UrlParameter.Optional }
          );
            //验证验证码
            routes.MapRoute(
              name: "checkregister",
              url: "checkregister.html",
              defaults: new { controller = "Loans", action = "checkregister", id = UrlParameter.Optional }
          );
            //个人申请贷款
            routes.MapRoute(
              name: "Borrloans",
              url: "Borrloans.html",
              defaults: new { controller = "Loans", action = "PostPersonLoans", id = UrlParameter.Optional }
          );
            //企业申请贷款
            routes.MapRoute(
              name: "Borrloans1",
              url: "Borrloans1.html",
              defaults: new { controller = "Loans", action = "Borrloans", id = UrlParameter.Optional }
          );


            routes.MapRoute(
               name: "Borrow",
               url: "Borrow.html",
               defaults: new { controller = "Borrow", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
             name: "investment_success",
             url: "investment_success_{id}.html",
             defaults: new { controller = "invest_borrow", action = "investment_success", id = UrlParameter.Optional }
         );
            routes.MapRoute(
              name: "Default",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
               namespaces: new string[] { "ChuanglitouP2P.Controllers" }
               );
            routes.MapRoute(
                       name: "adminDefault",
                       url: "admin/{controller}/{action}/{id}",
                       defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                       namespaces: new string[] { "ChuanglitouP2P.Areas.Admin.Controllers" }
            );
        }
    }
}
