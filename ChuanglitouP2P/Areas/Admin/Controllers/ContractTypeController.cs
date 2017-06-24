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
    public class ContractTypeController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();


        // GET: Admin/ContractType

        [AdminVaildate()]
        public ActionResult Index(string contract_type_name = "", int Page = 1, int pageSize = 10)
        {
            int pageNumber = Page / 1;
            Expression<Func<hx_contract_type, bool>> where = PredicateExtensionses.True<hx_contract_type>();
            where = where.And(p => p.contract_type_id > 0);

            if (!string.IsNullOrEmpty(contract_type_name))
            {
                where = where.And(p => p.contract_type_name.Contains(contract_type_name));
            }

            IPagedList<hx_contract_type> list = ef.hx_contract_type.Where(where).OrderByDescending(p => p.contract_type_id).ToPagedList(pageNumber, pageSize);

            ViewBag.contract_type_name = contract_type_name;
            ViewBag.page = Page;
            return View(list);
        }
        [HttpGet]
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
                return Content(StringAlert.Alert("参数错误", "/admin/ContractType/Index"), "text/html");
            }
            var model = ef.hx_contract_type.Where(p => p.contract_type_id == id).SingleOrDefault();
            if (model == null || model.contract_type_id < 1)
            {
                return Content(StringAlert.Alert("记录不存在", "/admin/ContractType/Index"), "text/html");
            }
            return View(model);
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult AddPost(hx_contract_type p)
        {
            p = (hx_contract_type)Utils.ValidateModelClass(p);

            ef.hx_contract_type.Add(p);
            //ef.SaveChanges();
            string str = "";
            int i = ef.SaveChanges();
            if (i > 0)
            {
                str = StringAlert.Alert("合同类型添加成功!", "/admin/ContractType/Index");
            }
            else
            {
                str = StringAlert.Alert("合同类型添加失败!", "/admin/ContractType/Add/");
            }
            return Content(str, "text/html");
        }
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult EditPost(hx_contract_type p)
        {
            string str = "";

            string[] proNames;

            proNames = new string[] { "contract_type_name" };


            p = (hx_contract_type)Utils.ValidateModelClass(p);


            DbEntityEntry entry = ef.Entry<hx_contract_type>(p);
            entry.State = EntityState.Unchanged;

            foreach (string ProName in proNames)
            {
                entry.Property(ProName).IsModified = true;
            }

            int i = ef.SaveChanges();
            if (i > 0)
            {
                str = StringAlert.Alert("合同类型修改成功!", "/Admin/ContractType/index");
            }
            else
            {
                str = StringAlert.Alert("合同类型修改失败!", "/admin/ContractType/Edit?id=" + p.contract_type_id);
            }
            return Content(str, "text/html");

        }


        [AdminVaildate()]
        public ActionResult DelById(int id, int Page = 1, string contract_type_name = "")
        {
            string str = "";

            hx_contract_type pDel = new hx_contract_type() { contract_type_id = id };
            ef.hx_contract_type.Attach(pDel);
            ef.hx_contract_type.Remove(pDel);
            int i = ef.SaveChanges();
            if (i > 0)
            {
                str = StringAlert.Alert("合同类型删除成功!", "/admin/ContractType/Index?page=" + Page.ToString() + "&contract_type_name=" + contract_type_name);
            }
            else
            {
                str = StringAlert.Alert("合同类型删除失败!", "/admin/ContractType/Index?page=" + Page.ToString() + "&contract_type_name=" + contract_type_name);
                
            }
            return Content(str, "text/html");

        }



    }
}