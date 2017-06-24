using System;
using ChuanglitouP2P.Common;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChuangLitouP2P.Models;
using ChuanglitouP2P.BLL.EF;
using System.Data.Entity;
using ChuanglitouP2P.BLL;
using System.IO;

namespace ChuanglitouP2P.Controllers
{
    public class HomeController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();

        private bool checkStart()
        {
            return DateTime.Now > TActivity_Luck.startTime && DateTime.Now <= TActivity_Luck.endTime;
        }
        public ActionResult Index()
        {
            var actFanXian = ef.hx_ActivityTable.Where(c => c.ActName == "12月投资立得返现奖励").OrderByDescending(c => c.ActEndtime).FirstOrDefault();
            ViewBag.FanXianSTime = actFanXian.ActStarttime;
            ViewBag.FanXianETime = actFanXian.ActEndtime;

            bool isPingguo = DateTime.Now.Millisecond % 2 == 1;
            string close_tanchu = checkStart() ? (isPingguo ? "style='top: 17%;background:url(/images/tan1260_3_close.png) no-repeat;'" : "style='top: 17%;background:url(/images/tan1260_4_close.png) no-repeat;'") : "";
            string tanchu = checkStart() ? (isPingguo ? "/Images/tan1260_3.png" : "/Images/tan1260_4.png") : "/Images/tan1260_1.png";
            ViewBag.Tanchu = tanchu;
            ViewBag.Close_tanchu = close_tanchu;
            string firstTimePerDay = Utils.GetCookie("FirstTimePerDay");
            ViewBag.FirstTimePerDay = string.IsNullOrWhiteSpace(firstTimePerDay) ? "block" : "none";
            if (string.IsNullOrWhiteSpace(firstTimePerDay))
            {
                var cookie = new HttpCookie("FirstTimePerDay", "That's ok");//加密身份信息，保存至Cookie
                cookie.Expires = DateTime.Now.Date.AddDays(1);
                Response.Cookies.Add(cookie);
            }
            int uid = Utils.checkloginsessiontop();
            hx_member_table HUsr = new hx_member_table();
            if (uid > 0)
            {
                HUsr = ef.hx_member_table.Where(p => p.registerid == uid).FirstOrDefault();
            }

            ViewBag.uid = uid;
            if (HUsr != null)
            {
                //  ViewBag.amt = HUsr.available_balance== null ? 0.00M : HUsr.available_balance;
                ViewBag.amt = HUsr.available_balance;
            }
            else
            {
                ViewBag.amt = 0.00;
            }
            hx_borrowing_target hbt = ef.hx_borrowing_target.Where(p => p.project_type_id == 6 && p.tender_state >= 2 && p.tender_state <= 5).OrderByDescending(p => p.targetid).Take(1).FirstOrDefault();
            List<V_borrowing_target_addlist> porlist = ef.V_borrowing_target_addlist.Where(p => p.tender_state >= 2 && p.tender_state <= 5 && p.project_type_id != 6
            //&& p.recommend==1
            ).OrderBy(p => p.tender_state).ThenByDescending(p => p.indexorder).ThenByDescending(p => p.targetid).Take(6).ToList();
            ViewBag.hbt = hbt;

            GetCacheNesList nes = new GetCacheNesList();
            ViewBag.newslist = nes.GetNews(17, 3, 1, 1); //媒体新闻三张列表图片
            ViewBag.news1list = nes.GetNews(21, 4, 0, 1);//公告
            ViewBag.newslist2 = nes.GetNews(23, 4, 0, 1);//行业新闻
            ViewBag.gonggao = nes.GetNews(21, 4, 0, 1); // 公告
            return View(porlist);
        }


        public JsonResult GetTotal()
        {
            string[] dt = B_usercenter.GetTotal();
            var result = new { state = 0, aLLFinance = dt[0], income = dt[1], investment = dt[2] };
            return Json(result);
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }


        public ActionResult tests()
        {
            return View();
        }


        public ActionResult Video()
        {
            return View();
        }

        public ActionResult App()
        {
            return View();
        }

        public ActionResult GetAndroidAPP()
        {
            B_AppUpdatePackage bll = new B_AppUpdatePackage();
            var cModel = bll.GetLastModel("Android", "CLT", "0.1");
            var model = bll.GetDownloadModel(cModel.Code);
            string androidUrl = string.Empty;
            if (model != null)
            {
                try
                {
                    string filePath = Settings.Instance.GetWebsitePhysicalRootPath + "\\" + model.VirtualPath.Replace("/", "\\");
                    FileStream fs = new FileStream(filePath, FileMode.Open);
                    return File(fs, "application/octet-stream", model.Code + ".apk");
                }
                catch (Exception ex)
                {
                    LogInfo.WriteLog("Android 服务器下载失败,未找到文件路径: " + ex.Message);
                    return Content("Android 最新版本还没有发布！");
                }
            }
            return View();
        }

    }
}