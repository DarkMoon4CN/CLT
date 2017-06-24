using ChuanglitouP2P.Areas.Admin.Controllers.Filters;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.Common.Extensionses;
using ChuangLitouP2P.Models;
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
    /// <summary>
    /// 友情链接管理
    /// </summary>
    public class LinkController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();

        // GET: Admin/Link
        [AdminVaildate()]
        public ActionResult Index(string linkname = "", int linktype = 0, int linkstate = 0, int Page = 1, int pageSize = 10)
        {
            int pageNumber = Page / 1;
            Expression<Func<hx_td_Links, bool>> where = PredicateExtensionses.True<hx_td_Links>();
            where = where.And(p => p.Linkid > 0);

           

            if (!string.IsNullOrEmpty(linkname))
            {
                where = where.And(p => p.Linkname.Contains(linkname));
            }
            if (linktype == 1)
            {
                where = where.And(p => p.LinkType == 0);
            }
            else if (linktype == 2)
            {
                where = where.And(p => p.LinkType == 1);
            }
            if (linkstate == 1)
            {
                where = where.And(p => p.Linkstate == 0);
            }
            else if (linkstate == 2)
            {
                where = where.And(p => p.Linkstate == 1);
            }
            IPagedList<hx_td_Links> list = ef.hx_td_Links.Where(where).OrderByDescending(p => p.Linkid).ToPagedList(pageNumber, pageSize);

            ViewBag.linkname = linkname;
            ViewBag.linktype = linktype;
            ViewBag.linkstate = linkstate;

            return View(list);
        }

        [AdminVaildate(false,true)]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [AdminVaildate()]
        public ActionResult AddLinkpost(hx_td_Links p)
        {

            p = (hx_td_Links)Utils.ValidateModelClass(p);
            p.LinkLogo.Replace("//", "/");
            p.createtime = DateTime.Now;
            ef.hx_td_Links.Add(p);
            ef.SaveChanges();
            CacheRemove.ReMovetd_web_Links();

            return RedirectToAction("index", "Link");
        }

        [HttpGet]
        [AdminVaildate()]
        public ActionResult Edit(int id, int Page=1)
        {
            if (id < 1)
            {
                return Content(StringAlert.Alert("参数错误", "/admin/Link/Index"), "text/html");
            }
            var model = ef.hx_td_Links.Where(p => p.Linkid == id).SingleOrDefault();
            if (model == null || model.Linkid < 1)
            {
                return Content(StringAlert.Alert("记录不存在", "/admin/Link/Index"), "text/html");
            }
            return View(model);
        }



        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [AdminVaildate()]
        public ActionResult Edit(hx_td_Links p)
        {
            string str = "";
            int page = DNTRequest.GetInt("page", 1);
            string[] proNames;

            proNames = new string[] { "Linkname", "LinkUrl", "LinkType", "LinkLogo", "Linkstate" };
            p = (hx_td_Links)Utils.ValidateModelClass(p);


            p.LinkLogo.Replace("//", "/");
            DbEntityEntry entry = ef.Entry<hx_td_Links>(p);
            entry.State = EntityState.Unchanged;

            foreach (string ProName in proNames)
            {
                entry.Property(ProName).IsModified = true;
            }
            int i = ef.SaveChanges();
            if (i > 0)
            {
                str = StringAlert.Alert("友情链接修改成功!", "/admin/Link?page=" + page.ToString());
            }
            else
            {
                str = StringAlert.Alert("友情链接修改失败!", "/admin/Link/Edit/" + p.Linkid + "/");
            }
            CacheRemove.ReMovetd_web_Links();

            return Content(str, "text/html");

        }

        [AdminVaildate()]
        public ActionResult DelById(int id, int Page=1)
        {
            string str = "";

            string linkname = DNTRequest.GetString("linkname");
            hx_td_Links pDel = new hx_td_Links() { Linkid = id };
            ef.hx_td_Links.Attach(pDel);
            ef.hx_td_Links.Remove(pDel);
            int i = ef.SaveChanges();
            if (i > 0)
            {

                str = StringAlert.Alert("友情链接删除成功!", "/admin/Link?page=" + Page.ToString() + "&linkname=" + linkname);

                CacheRemove.ReMovetd_web_Links();
            }
            else
            {
                str = StringAlert.Alert("友情链接删除失败!", "/admin/Link?page=" + Page.ToString());
            }
          
            return Content(str, "text/html");

        }
    }
}