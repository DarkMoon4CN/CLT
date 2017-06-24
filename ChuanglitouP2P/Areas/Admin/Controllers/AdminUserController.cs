using ChuanglitouP2P.Common.Extensionses;
using ChuangLitouP2P.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using EntityFramework.Extensions;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.BLL.EF;
using ChuanglitouP2P.DBUtility;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using ChuanglitouP2P.Areas.Admin.Controllers.Filters;

namespace ChuanglitouP2P.Areas.Admin.Controllers
{
    /// <summary>
    /// 后台用户信息
    /// </summary>
    public class AdminUserController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();
        // GET: Admin/AdminUser
        [AdminVaildate()]
        public ActionResult Index(string username = "", int state = 0)
        {
            Expression<Func<v_adminuser_department, bool>> where = PredicateExtensionses.True<v_adminuser_department>();
            where = where.And(p => p.adminuserid > 0);
            if (!string.IsNullOrEmpty(username))
            {
                where = where.And(p => p.adminuser.Contains(username));
            }
            if (state > 0)
            {
                where = where.And(p => p.state == state);
            }
            List<v_adminuser_department> list = ef.v_adminuser_department.Where(where).OrderByDescending(a => a.adminuserid).ToList();

            ViewBag.username = username;
            ViewBag.state = state;

            return View(list);
        }

        //删除
        [AdminVaildate()]
        public ActionResult remove(int id)
        {
            var result = ef.hx_td_adminuser.Where(a => a.adminuserid == id).Delete();
            if (result > 0)
            {
                return Content(StringAlert.Alert("操作成功", "/admin/AdminUser/Index"), "text/html");
            }
            else
            {
                return Content(StringAlert.Alert("操作失败"), "text/html");
            }
        }

        #region 新增

        [AdminVaildate()]
        public ActionResult Add()
        {

            ViewBag.department = new SelectListByEF().GetDepartmentDropDownList(0, "0", "请选择");

            return View();
        }

        public ActionResult CheckName(string param, int key = 0)
        {
            var item = (from a in ef.hx_td_adminuser where a.adminuserid != key && a.adminuser == param select a).SingleOrDefault();
            if (item != null && item.adminuserid > 0)
            {
                return Content("管理员账号已存在");
            }
            return Content("y");
        }


        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Add(hx_td_adminuser t)
        {
            var userpass = t.userpass;
            t = (hx_td_adminuser)Utils.ValidateModelClass(t);

            //  t.userpass = DESEncrypt.Encrypt(userpass.Trim(), ConfigurationManager.AppSettings["KEYS"].ToString());

            t.userpass = Utils.MD5(userpass.Trim());
            t.province = "0";
            t.city = "0";
            t.email = "";
            t.datetime = DateTime.Now;
            t.lastLoginTime = DateTime.Now;
            t.loginTimes = 0;
            t.area_id = 0;

            ef.hx_td_adminuser.Add(t);

            int id = ef.SaveChanges();
            if (id > 0)
            {
                return Content(StringAlert.Alert("操作成功","/admin/AdminUser/index"), "text/html");
            }
            else
            {
                return Content(StringAlert.Alert("操作失败"), "text/html");
            }
        }

        #endregion

        #region 编辑

        [AdminVaildate()]
        public ActionResult Editor(int id)
        {
            hx_td_adminuser model = (from a in ef.hx_td_adminuser where a.adminuserid == id select a).SingleOrDefault();

            ViewBag.department = new SelectListByEF().GetDepartmentDropDownList(0, "0", "请选择");
            ViewBag.id = id;

            return View(model);
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Editor(hx_td_adminuser p)
        {
            string[] proNames;
            if (string.IsNullOrEmpty(p.userpass))
            {
                proNames = new string[] { "trueName", "worknum", "sex", "tel", "phone_number", "department_id","state" };
            }
            else
            {
                p.userpass= Utils.MD5(p.userpass);
                proNames = new string[] { "trueName", "worknum", "sex", "tel", "phone_number", "department_id","state","userpass" };
            }
            p = (hx_td_adminuser)Utils.ValidateModelClass(p);
                     

            DbEntityEntry entry = ef.Entry<hx_td_adminuser>(p);
            entry.State = EntityState.Unchanged;

            foreach (string ProName in proNames)
            {
                entry.Property(ProName).IsModified = true;
            }
            int i = ef.SaveChanges();
            if (i > 0)
            {
                return Content(StringAlert.Alert("操作成功", "/admin/AdminUser/index"), "text/html");
            }
            else
            {
                return Content(StringAlert.Alert("操作失败"), "text/html");
            }
        }

        #endregion
    }
}