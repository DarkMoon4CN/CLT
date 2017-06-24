using ChuanglitouP2P.Areas.Admin.Controllers.Filters;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.Common.Extensionses;
using ChuangLitouP2P.Models;
using EntityFramework.Extensions;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace ChuanglitouP2P.Areas.Admin.Controllers
{
    public class HelpController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();

        // GET: Admin/Help
        [AdminVaildate()]
        public ActionResult Index(string news_title = "", int rootid = 0, int Page = 1, int pageSize = 10)
        {
            int pageNumber = Page / 1;
            Expression<Func<V_type_news, bool>> where = PredicateExtensionses.True<V_type_news>();
            where = where.And(p => p.newid > 0);

            where = where.And(p => p.rootid == rootid);

            if (!string.IsNullOrEmpty(news_title))
            {
                where = where.And(p => p.News_title.Contains(news_title));
            }

            IPagedList<V_type_news> list = ef.V_type_news.Where(where).OrderByDescending(p => p.newid).ToPagedList(pageNumber, pageSize);

            ViewBag.news_title = news_title;
            ViewBag.rootid = rootid;
            ViewBag.page = Page;
            return View(list);
        }
        [HttpGet]
        // GET: Admin/Help
        [AdminVaildate(false,true)]
        public ActionResult Add(int rootid = 0)
        {
            ViewBag.rootid = rootid;

           
            var list_web_type = (from a in ef.hx_td_web_type where a.parentid == rootid select a).ToList<hx_td_web_type>();
            ViewBag.web_types = list_web_type;

            return View();
        }
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [AdminVaildate()]
        public ActionResult AddHelpPost(hx_td_about_news p,int rootid, string context)
        {

            p = (hx_td_about_news)Utils.ValidateModelClass(p);
            p.context = context;
            p.createtime = DateTime.Now;
            p.newimg = p.newimg.Replace("//", "/");

            p.comm = 0;
            p.listcomm = 0;
            p.adminuserid = 0;
            ef.hx_td_about_news.Add(p);
            //ef.SaveChanges();
            string str = "";
            int i = ef.SaveChanges();
            if (i > 0)
            {
                CacheRemove.RemoveWebCache("NewsList");

                str = StringAlert.Alert("添加成功!", "/admin/Help/Index?rootid=" + rootid);
            }
            else
            {
                str = StringAlert.Alert("添加失败!", "/admin/Help/Add?rootid=" + rootid);
            }
            return Content(str, "text/html");

        }

        [HttpGet]
        [AdminVaildate()]
        // GET: Admin/Help
        public ActionResult Edit(int id = 0, int rootid = 0, int page = 1)
        {
            if (id < 1)
            {
                return Content(StringAlert.Alert("参数错误", "/admin/Help/Index?rootid=" + rootid), "text/html");
            }

            ViewBag.rootid = rootid;
            var model = ef.hx_td_about_news.Where(p => p.newid == id).SingleOrDefault();
            if (model == null || model.newid < 1)
            {
                return Content(StringAlert.Alert("记录不存在", "/admin/Help/Index?rootid=" + rootid), "text/html");
            }

            var list_web_type = (from a in ef.hx_td_web_type where a.parentid == rootid select a).ToList<hx_td_web_type>();
            ViewBag.web_types = list_web_type;
            ViewBag.page = page;
            
            return View(model);
        }
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [AdminVaildate(false,true)]
        public ActionResult EditHelpPost(hx_td_about_news p, int rootid = 0, int page = 1, string context="")
        {
            string str = "";

            string[] proNames;

            proNames = new string[] { "News_title", "web_Type_menu_id", "News_Key", "news_Des", "context" };


            p = (hx_td_about_news)Utils.ValidateModelClass(p);

            p.context = context;
            p.newimg = p.newimg.Replace("//", "/");
            DbEntityEntry entry = ef.Entry<hx_td_about_news>(p);
            entry.State = EntityState.Unchanged;

            foreach (string ProName in proNames)
            {
                entry.Property(ProName).IsModified = true;
            }

            int i = ef.SaveChanges();
            if (i > 0)
            {
                CacheRemove.RemoveWebCache("NewsList");
                str = StringAlert.Alert("帮助中心修改成功!", "/Admin/help/index?page=" + page + "&rootid=" + rootid);
            }
            else
            {
                str = StringAlert.Alert("帮助中心修改失败!", "/admin/help/Edit?id=" + p.newid + "&rootid=" + rootid + "&page=" + page);
            }
            return Content(str, "text/html");
        }


        public ActionResult DelById(int id, int Page = 1, int rootid = 0,string news_title="")
        {
            string str = "";

            hx_td_about_news pDel = new hx_td_about_news() { newid = id };
            ef.hx_td_about_news.Attach(pDel);
            ef.hx_td_about_news.Remove(pDel);
            int i = ef.SaveChanges();
            if (i > 0)
            {
                CacheRemove.RemoveWebCache("NewsList");

                str = StringAlert.Alert("帮助中心删除成功!", "/admin/Help/Index?page=" + Page.ToString() + "&rootid=" + rootid.ToString() + "&news_title=" + news_title);
            }
            else
            {
                str = StringAlert.Alert("帮助中心删除失败!", "/admin/Help/Index?page=" + Page.ToString() + "&rootid=" + rootid.ToString() + "&news_title=" + news_title);
            }
            return Content(str, "text/html");

        }



        #region 操作状态处理
        /// <summary>
        /// 操作状态处理
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [AdminVaildate(false, true)]
        public ActionResult SetCommtate(int id, int state,int commtype)
        {
            string json = "{\"ret\":0,\"msg\":\"操作失败\"}";
            if (id < 1)
            {
                json = "{\"ret\":-1,\"msg\":\"参数错误\"}";
            }
            else
            {
                var i = 0;
                
                if(commtype==1)
                {
                    i = ef.hx_td_about_news.Where(a => a.newid == id).Update(a => new hx_td_about_news { comm = state });   //首页推荐
                }
                else
                {
                    i = ef.hx_td_about_news.Where(a => a.newid == id).Update(a => new hx_td_about_news { listcomm = state });   //首页推荐
                }
              
               
                if (i > 0)
                {
                    CacheRemove.RemoveWebCache("NewsList");
                    json = "{\"ret\":1,\"msg\":\"操作成功\"}";
                }
            }

            return Content(json, "text/json");

        }
        #endregion


    }
}