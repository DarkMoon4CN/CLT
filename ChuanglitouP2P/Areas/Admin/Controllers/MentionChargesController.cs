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
    public class MentionChargesController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();

        // GET: Admin/MentionCharges
        [AdminVaildate(false, true)]
        public ActionResult Index(string mention_charges_name = "", int fees_unit = -1, int Page = 1, int pageSize = 10)
        {
            int pageNumber = Page / 1;
            Expression<Func<hx_Mention_charges, bool>> where = PredicateExtensionses.True<hx_Mention_charges>();
            where = where.And(p => p.mention_charges_id > 0);

            if (fees_unit > -1)
            {
                where = where.And(p => p.fees_unit == fees_unit);
            }


            if (!string.IsNullOrEmpty(mention_charges_name))
            {
                where = where.And(p => p.mention_charges_name.Contains(mention_charges_name));
            }

            IPagedList<hx_Mention_charges> list = ef.hx_Mention_charges.Where(where).OrderByDescending(p => p.mention_charges_id).ToPagedList(pageNumber, pageSize);

            ViewBag.mention_charges_name = mention_charges_name;
            ViewBag.fees_unit = fees_unit;
            ViewBag.page = Page;
            return View(list);
        }


        [AdminVaildate(false, true)]
        public ActionResult Add()
        {
            return View();
        }

        [HttpGet]
        [AdminVaildate(false, true)]
        public ActionResult Edit(int id = 0)
        {

            var model = ef.hx_Mention_charges.Where(p => p.mention_charges_id == id).SingleOrDefault();

            return View(model);
        }
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [AdminVaildate(false, true)]
        public ActionResult AddPost(hx_Mention_charges p)
        {

            p = (hx_Mention_charges)Utils.ValidateModelClass(p);
            ef.hx_Mention_charges.Add(p);
            //ef.SaveChanges();
            string str = "";
            int i = ef.SaveChanges();
            if (i > 0)
            {
                str = StringAlert.Alert("提现费用添加成功!", "/admin/MentionCharges/Index");
            }
            else
            {
                str = StringAlert.Alert("提现费用添加失败!", "/admin/MentionCharges/Add/");
            }
            return Content(str, "text/html");

        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [AdminVaildate(false, true)]
        public ActionResult EditPost(hx_Mention_charges p, int page = 1)
        {
            string str = "";

            string[] proNames;

            proNames = new string[] { "mention_charges_name", "minimum_amount", "maximum_amount", "fees", "fees_unit" };


            p = (hx_Mention_charges)Utils.ValidateModelClass(p);


            DbEntityEntry entry = ef.Entry<hx_Mention_charges>(p);
            entry.State = EntityState.Unchanged;

            foreach (string ProName in proNames)
            {
                entry.Property(ProName).IsModified = true;
            }

            int i = ef.SaveChanges();
            if (i > 0)
            {
                str = StringAlert.Alert("提现费用修改成功!", "/Admin/MentionCharges/index?page=" + page);
            }
            else
            {
                str = StringAlert.Alert("提现费用修改失败!", "/admin/MentionCharges/Edit?id=" + p.mention_charges_id);
            }
            return Content(str, "text/html");
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Page"></param>
        /// <param name="rootid"></param>
        /// <param name="news_title"></param>
        /// <returns></returns>
        [AdminVaildate(false, true)]
        public ActionResult DelById(int id, int Page = 1, int fees_unit = -1, string mention_charges_name = "")
        {
            string str = "";

            hx_Mention_charges pDel = new hx_Mention_charges() { mention_charges_id = id };
            ef.hx_Mention_charges.Attach(pDel);
            ef.hx_Mention_charges.Remove(pDel);
            int i = ef.SaveChanges();
            if (i > 0)
            {

                str = StringAlert.Alert("提现费用删除成功!", "/admin/MentionCharges/Index?page=" + Page.ToString() + "&fees_unit=" + fees_unit.ToString() + "&mention_charges_name=" + mention_charges_name);
            }
            else
            {
                str = StringAlert.Alert("提现费用删除失败!", "/admin/MentionCharges/Index?page=" + Page.ToString() + "&fees_unit=" + fees_unit.ToString() + "&mention_charges_name=" + mention_charges_name);
            }
            return Content(str, "text/html");

        }



    }
}