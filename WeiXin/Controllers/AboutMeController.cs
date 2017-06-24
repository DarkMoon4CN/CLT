using ChuangLitouP2P.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace WeiXin.Controllers
{
    public class AboutMeController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();

        /// <summary>
        /// 联系我们
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 公告
        /// </summary>
        /// <returns></returns>
        public ActionResult List(int id, int? pageIndex, int pagesize = 10)
        {
            string title = "关于我们";

            hx_td_web_type pr = ef.hx_td_web_type.Where(c => c.menu_id == id).FirstOrDefault();

            if (pr != null)
            {
                title = pr.menu_name;
            }
            var list = ef.V_type_news.Where(p => p.web_Type_menu_id == id).OrderByDescending(p => p.listcomm).ThenByDescending(p => p.newid).ToPagedList(pageIndex ?? 1, pagesize);

            ViewBag.title1 = title;
            ViewBag.id = id;


            if (Request.IsAjaxRequest())
                return PartialView("_aboutList", list);
            return View(list);



        }

        public ActionResult Detail(int id, string title = "")
        {
            V_type_news pr = ef.V_type_news.Where(p => p.newid == id).FirstOrDefault();

            ViewBag.title = string.IsNullOrWhiteSpace(title) ? pr.menu_name : title;

            return View(pr);
        }

        /// <summary>
        /// 联系我们
        /// </summary>
        /// <returns></returns>
        public ActionResult Company()
        {
            return View();
        }

    }
}