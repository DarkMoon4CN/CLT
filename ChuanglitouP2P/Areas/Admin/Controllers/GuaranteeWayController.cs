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
    public class GuaranteeWayController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();

        // GET: Admin/GuaranteeWay
        [AdminVaildate()]
        public ActionResult Index(string guarantee_way_name = "", int Page = 1, int pageSize = 10)
        {
            int pageNumber = Page / 1;
            Expression<Func<guarantee_way, bool>> where = PredicateExtensionses.True<guarantee_way>();
            where = where.And(p => p.guarantee_way_id > 0);

            if (!string.IsNullOrEmpty(guarantee_way_name))
            {
                where = where.And(p => p.guarantee_way_name.Contains(guarantee_way_name));
            }

            IPagedList<guarantee_way> list = ef.guarantee_way.Where(where).OrderByDescending(p => p.guarantee_way_id).ToPagedList(pageNumber, pageSize);

            ViewBag.guarantee_way_name = guarantee_way_name;
            ViewBag.page = Page;
            return View(list);
        }
        [AdminVaildate()]
        public ActionResult Add()
        {
            return View();
        }
        [HttpGet]
        [AdminVaildate()]
        public ActionResult Edit(int id = 0)
        {
            if (id < 1)
            {
                return Content(StringAlert.Alert("参数错误", "/admin/GuaranteeWay/Index"), "text/html");
            }
            var model = ef.guarantee_way.Where(p => p.guarantee_way_id == id).SingleOrDefault();
            if (model == null || model.guarantee_way_id < 1)
            {
                return Content(StringAlert.Alert("记录不存在", "/admin/GuaranteeWay/Index"), "text/html");
            }
            return View(model);
        }
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult AddPost(guarantee_way p)
        {
            p = (guarantee_way)Utils.ValidateModelClass(p);
            p.createtime = DateTime.Now;
            ef.guarantee_way.Add(p);
            //ef.SaveChanges();
            string str = "";
            int i = ef.SaveChanges();
            if (i > 0)
            {
                str = StringAlert.Alert("担保方式添加成功!", "/admin/GuaranteeWay/Index");
            }
            else
            {
                str = StringAlert.Alert("担保方式添加失败!", "/admin/GuaranteeWay/Add/");
            }
            return Content(str, "text/html");
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult EditPost(guarantee_way p)
        {
            string str = "";

            string[] proNames;

            proNames = new string[] { "guarantee_way_name", "amount_guaranteed" };


            p = (guarantee_way)Utils.ValidateModelClass(p);


            DbEntityEntry entry = ef.Entry<guarantee_way>(p);
            entry.State = EntityState.Unchanged;

            foreach (string ProName in proNames)
            {
                entry.Property(ProName).IsModified = true;
            }

            int i = ef.SaveChanges();
            if (i > 0)
            {
                str = StringAlert.Alert("担保方式修改成功!", "/Admin/GuaranteeWay/index");
            }
            else
            {
                str = StringAlert.Alert("担保方式修改失败!", "/admin/GuaranteeWay/Edit?id=" + p.guarantee_way_id);
            }
            return Content(str, "text/html");

        }


        [AdminVaildate()]
        public ActionResult DelById(int id, int Page = 1, string guarantee_way_name = "")
        {
            string str = "";

            guarantee_way pDel = new guarantee_way() { guarantee_way_id = id };
            ef.guarantee_way.Attach(pDel);
            ef.guarantee_way.Remove(pDel);
            int i = ef.SaveChanges();
            if (i > 0)
            {
                str = StringAlert.Alert("担保方式删除成功!", "/admin/GuaranteeWay/Index?page=" + Page.ToString() + "&guarantee_way_name=" + guarantee_way_name);
            }
            else
            {
                str = StringAlert.Alert("担保方式删除失败!", "/admin/GuaranteeWay/Index?page=" + Page.ToString() + "&guarantee_way_name=" + guarantee_way_name);
            }
            return Content(str, "text/html");

        }


    }
}