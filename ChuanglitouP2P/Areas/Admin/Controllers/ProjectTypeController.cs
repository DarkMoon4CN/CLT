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
    public class ProjectTypeController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();

        // GET: Admin/ProjectType
        [AdminVaildate(false, true)]
        public ActionResult Index(string project_type_name = "", int Page = 1, int pageSize = 10)
        {
            int pageNumber = Page / 1;
            Expression<Func<hx_Project_type, bool>> where = PredicateExtensionses.True<hx_Project_type>();
            where = where.And(p => p.project_type_id > 0);

            if (!string.IsNullOrEmpty(project_type_name))
            {
                where = where.And(p => p.project_type_name.Contains(project_type_name));
            }

            IPagedList<hx_Project_type> list = ef.hx_Project_type.Where(where).OrderByDescending(p => p.project_type_id).ToPagedList(pageNumber, pageSize);

            ViewBag.project_type_name = project_type_name;
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
                return Content(StringAlert.Alert("参数错误", "/admin/ProjectType/Index"), "text/html");
            }
            var model = ef.hx_Project_type.Where(p => p.project_type_id == id).SingleOrDefault();
            if (model == null || model.project_type_id < 1)
            {
                return Content(StringAlert.Alert("记录不存在", "/admin/ProjectType/Index"), "text/html");
            }
            return View(model);
        }
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [AdminVaildate(false, true)]
        public ActionResult AddPost(hx_Project_type p)
        {
            p = (hx_Project_type)Utils.ValidateModelClass(p);
            
            ef.hx_Project_type.Add(p);
            //ef.SaveChanges();
            string str = "";
            int i = ef.SaveChanges();
            if (i > 0)
            {
                str = StringAlert.Alert("项目类型添加成功!", "/admin/ProjectType/Index");
            }
            else
            {
                str = StringAlert.Alert("项目类型添加失败!", "/admin/ProjectType/Add/");
            }
            return Content(str, "text/html");
        }
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [AdminVaildate(false, true)]
        public ActionResult EditPost(hx_Project_type p)
        {
            string str = "";

            string[] proNames;

            proNames = new string[] { "project_type_name"};


            p = (hx_Project_type)Utils.ValidateModelClass(p);

           
            DbEntityEntry entry = ef.Entry<hx_Project_type>(p);
            entry.State = EntityState.Unchanged;

            foreach (string ProName in proNames)
            {
                entry.Property(ProName).IsModified = true;
            }

            int i = ef.SaveChanges();
            if (i > 0)
            {
                str = StringAlert.Alert("项目类型修改成功!", "/Admin/ProjectType/index");
            }
            else
            {
                str = StringAlert.Alert("项目类型修改失败!", "/admin/ProjectType/Edit?id=" + p.project_type_id);
            }
            return Content(str, "text/html");

        }


        [AdminVaildate()]
        public ActionResult DelById(int id, int Page = 1, string project_type_name = "")
        {
            string str = "";

            hx_Project_type pDel = new hx_Project_type() { project_type_id = id };
            ef.hx_Project_type.Attach(pDel);
            ef.hx_Project_type.Remove(pDel);
            int i = ef.SaveChanges();
            if (i > 0)
            {
                str = StringAlert.Alert("项目类型删除成功!", "/admin/ProjectType/Index?page=" + Page.ToString() + "&project_type_name=" + project_type_name);
            }
            else
            {
                str = StringAlert.Alert("项目类型删除失败!", "/admin/ProjectType/Index?page=" + Page.ToString() + "&project_type_name=" + project_type_name);
            }
            return Content(str, "text/html");

        }

    }
}