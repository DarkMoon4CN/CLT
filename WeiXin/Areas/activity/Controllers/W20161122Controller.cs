using ChuanglitouP2P.BLL;
using ChuangLitouP2P.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WeiXin.Areas.activity.Controllers
{
    public class W20161122Controller : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();
        // GET: activity/W20161122
        public ActionResult Index()
        {
            int userid =ChuanglitouP2P.Common.Settings.Instance.CurrentUserId;
            ViewBag.UserID = userid;
            string app=Request["app"];
            string appUserid = Request["userid"];
            appUserid= appUserid == null ? "0" : appUserid;
            string ms = string.Empty;
            string af =string.Empty;
            int isApp = 0;
            int rid = Convert.ToInt32(appUserid);
            var model=ef.hx_member_table.Where(p => p.registerid == rid).FirstOrDefault();
            if (model != null && !string.IsNullOrWhiteSpace(model.channel_invitedcode))
            {
                af = "<a  tag='key' class='w_xianjin_zhuce' >渠道用户不参与</a> ";
            }
            else if (!string.IsNullOrWhiteSpace(app) && app == "clt" && appUserid != "0")
            {
                isApp = 1;
                ms = "#home";
                af = "<a href='Javascript:void(0)' tag='key' class='w_xianjin_zhuce' id='w_xianjin_zhuce'>注册领红包</a> ";
            }
            else if(!string.IsNullOrWhiteSpace(app) && app == "clt" && appUserid == "0") 
            {
                isApp = 1;
                ms = "#register";
                af = "<a href='#login' class='w_xianjin_zhuce' >注册领红包</a> ";
            }
            else
            {
                isApp = 0;
                af = "<a href='/DownLoad.html' class='w_xianjin_zhuce' >注册领红包</a> ";
                ms = "/DownLoad.html";
            }
            ViewBag.isApp = isApp;
            ViewBag.af = af;
            ViewBag.ms = ms;
            return View();
        }
    }
}