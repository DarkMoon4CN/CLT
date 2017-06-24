using ChuanglitouP2P.BLL.EF;
using ChuanglitouP2P.Common;
using ChuangLitouP2P.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using System.Web.UI;
using Webdiyer.WebControls.Mvc;

namespace ChuanglitouP2P.Controllers
{
    public class aboutController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();
        // GET: about
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult Index()
        {
            GetCacheNesList nes = new GetCacheNesList();
            ViewBag.newslist = nes.GetNews(17, 3, 1); //媒体新闻三张列表图片
            ViewBag.newslists = nes.GetNews(17, 9, 0); //媒体新闻列表图片
            ViewBag.gonggao = nes.GetNews(21, 10, 0);  // 公告

            ViewBag.newslistimg = nes.GetNews(23, 4, 1); //行业新闻三张列表图片
            ViewBag.hongyenewslist = nes.GetNews(23, 5, 0);  // 行业新闻
            ViewBag.asks = nes.GetNews(22, 5, 0);  // 常见问题
            return View();
        }

        /// <summary>
        /// 关于我们列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List(int id, int? pageIndex, int pgaesize = 10)
        {
            // List<V_type_news> list = ef.V_type_news.Where(p => p.web_Type_menu_id == id).OrderByDescending(p => p.newid).ToList();

            string title = "关于我们";

            hx_td_web_type pr = ef.hx_td_web_type.Where(c => c.menu_id == id).FirstOrDefault();

            if (pr != null)
            {

                title = pr.menu_name;
            }


            var list = ef.V_type_news.Where(p => p.web_Type_menu_id == id).OrderByDescending(p => p.listcomm).ThenByDescending(p => p.newid).ToPagedList(pageIndex ?? 1, pgaesize);

            ViewBag.title1 = title;
            ViewBag.MenuId = id;

            if (Request.IsAjaxRequest())
                return PartialView("_aboutList", list);
            GetLink(id, "List"); //获取导航
            ViewBag.Link = Namelist;
            return View(list);
        }




        public ActionResult Detail(int id)
        {
            V_type_news pr = ef.V_type_news.Where(p => p.newid == id).FirstOrDefault();

            if (pr != null)
            {
                ViewBag.title1 = pr.News_title;
                ViewBag.updateTime =Convert.ToDateTime(pr.createtime).ToString("yyyy-MM-dd");
                ViewBag.ClickCount = pr.ClickCount;
            }
            else
            {
                ViewBag.title1 = "关于我们";
            }
            GetLink(id, "Detail"); //获取导航
            ViewBag.Link = Namelist;

            return View(pr);
        }

        string Namelist = string.Empty;
        /// <summary>
        /// 面包屑导航
        /// </summary>
        private void GetLink(int id, string type)
        {
            if (type == "Detail")
            {
                V_type_news vpn = ef.V_type_news.Where(p => p.newid == id).FirstOrDefault();
                if (vpn != null)
                {
                    if (Namelist == "")
                    {
                        id = Convert.ToInt32(vpn.web_Type_menu_id);
                        Namelist = "<a style=\"cursor: pointer; \"> " + vpn.News_title + " </a>";
                    }
                }
            }
            hx_td_web_type pr = ef.hx_td_web_type.Where(c => c.menu_id == id).FirstOrDefault();
            if (pr != null)
            {
                if (Namelist != "")
                {
                    if (id == 3) //如果是 关于我们
                    {
                        Namelist = "<a style=\"cursor: pointer; \" href=\"/about.html\"> " + pr.menu_name + " </a>&gt;" + "<a style=\"cursor: pointer; \"> " + Namelist + " </a>";
                    }
                    else
                    {
                        Namelist = "<a style=\"cursor: pointer; \" href=\"/about/List/" + id + "\"> " + pr.menu_name + " </a>&gt;" + "<a style=\"cursor: pointer; \"> " + Namelist + " </a>";
                    }
                }
                else
                {
                    Namelist = "<a style=\"cursor: pointer; \"> " + pr.menu_name + " </a>";
                }
                if (pr.parentid != 0)
                {
                    GetLink(Convert.ToInt32(pr.parentid), type);
                }
            }
        }
    }
}