using ChuangLitouP2P.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityFramework.Extensions;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.BLL.EF;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using ChuanglitouP2P.Areas.Admin.Controllers.Filters;

namespace ChuanglitouP2P.Areas.Admin.Controllers
{
    /// <summary>
    /// 部门信息
    /// </summary>
    public class DeparmentController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();
        // GET: Admin/Deparment
        [AdminVaildate(false, true)]
        public ActionResult Index()
        {
            List<IGrouping<int, hx_td_department>> groupList = ef.hx_td_department.OrderByDescending(j =>(int) j.orderid).GroupBy(m => (int)m.parentid).ToList();
            Dictionary<int, List<hx_td_department>> dic = new Dictionary<int, List<hx_td_department>>();
            if (groupList != null && groupList.Count() > 0)
            {
                foreach( var group in groupList)
                {
                    dic.Add(group.Key, group.ToList());
                }
                
            }

           // var groupList = ef.hx_td_department.OrderBy(p => p.parentid).ThenByDescending(p => p.orderid);

            //Dictionary<int, List<hx_td_department>> dic = new Dictionary<int, List<hx_td_department>>();
            //if (groupList != null && groupList.Count() > 0)
            //{
            //    List<hx_td_department> tempList = new List<hx_td_department>();
            //    int parentid = -1;
            //    foreach (var item in groupList)
            //    {
            //        if (item.parentid != null)
            //        {
            //            if (parentid != -1 && parentid == item.parentid)
            //            {
            //                parentid = item.parentid.Value;
            //                tempList.Add(item);
            //            }
            //            else if (parentid != -1 && parentid != item.parentid)
            //            {
            //                dic.Add(parentid, tempList);
            //                parentid = item.parentid.Value;
            //                tempList = new List<hx_td_department>();
            //                tempList.Add(item);
            //            }
            //            else
            //            {
            //                tempList.Add(item);
            //                parentid = item.parentid.Value;
            //            }
            //        }
            //    }
            //    if (tempList.Count > 0)
            //        dic.Add(parentid, tempList);
            //    tempList.Clear(); tempList = null;
            //    GC.Collect();
            //}
            return View(dic);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [AdminVaildate()]
        public ActionResult Remove(int id)
        {
            if (id < 1)
            {
                return Content(StringAlert.Alert("参数错误", "/admin/Deparment/Index"), "text/html");
            }
            var result = ef.hx_td_department.Where(a => a.department_id == id).Delete();

            if (result > 0)
            {
                new SelectListByEF().ClearDepartmentInfo();
                new UserLimitByEF().ClearDepartmentCacheKey(id);
                ef.hx_DepUserLimit1.Where(a => a.departmentId == id).Delete();
                return Content(StringAlert.Alert("操作成功", "/admin/Deparment/Index"), "text/html");
            }
            else
            {
                return Content(StringAlert.Alert("操作失败", "/admin/Deparment/Index"), "text/html");
            }
        }

        #region 修改

        [AdminVaildate()]
        public ActionResult Editor(int id)
        {
            hx_td_department item = (from a in ef.hx_td_department where a.department_id == id select a).SingleOrDefault();

            ViewBag.dropdown = new SelectListByEF().GetDepartmentDropDownList(id, "0", "添加为根栏目");
            ViewBag.id = id;
            ViewBag.name = item.department_name;
            ViewBag.parentid = item.parentid;

            return View();
        }

        public ActionResult SaveEditor(int key, int parendid, string name)
        {
            name = Utils.CheckSQLHtml(HttpUtility.UrlDecode(name));
            var list = (from a in ef.hx_td_department where a.department_name == name && a.department_id != key select a).SingleOrDefault();
            if (list != null && list.department_id > 0)
            {
                return Content("{\"ret\":-1,\"msg\":\"部门名称已经存在\"}", "text/json");
            }

            hx_td_department model = new hx_td_department();
            model.department_name = name;
            model.parentid = parendid;
            model.department_id = key;

            if (parendid != 0)
            {
                var parent = (from a in ef.hx_td_department where a.department_id == parendid select a).SingleOrDefault();
                if (parent != null)
                {
                    model.rootid = parent.rootid;
                    model.parentpath = parent.parentpath+","+model.department_id+",";
                    model.depath = parent.depath + 1;
                }
            }

            string[] proNames;
            proNames = new string[] { "department_name", "parentid" , "parentpath" ,"rootid","depath"};

            DbEntityEntry entry = ef.Entry<hx_td_department>(model);
            entry.State = EntityState.Unchanged;

            foreach (string ProName in proNames)
            {
                entry.Property(ProName).IsModified = true;
            }
            int result = ef.SaveChanges();
            if (result > 0)
            {
                new SelectListByEF().ClearDepartmentInfo();
                return Content("{\"ret\":1,\"msg\":\"保存成功！\"}", "text/json");
            }
            else
            {
                return Content("{\"ret\":0,\"msg\":\"保存失败！\"}", "text/json");
            }

        }

        #endregion

        #region 新增

        [AdminVaildate()]
        public ActionResult Add()
        {
            ViewBag.dropdown = new SelectListByEF().GetDepartmentDropDownList(0, "0", "添加为根栏目");

            return View();
        }

        public ActionResult SaveNew(int parendid, string name)
        {
            name = Utils.CheckSQLHtml(HttpUtility.UrlDecode(name));
            var item = (from a in ef.hx_td_department where a.department_name == name select a).SingleOrDefault();
            if (item != null && item.department_id > 0)
            {
                return Content("{\"ret\":-1,\"msg\":\"部门名称已经存在\"}", "text/json");
            }
            
            hx_td_department model = new hx_td_department();
            model.createtime = DateTime.Now;
            model.department_name = name;
            model.parentid = parendid;
            model.orderid = 0;
            model.previd = 0;
            model.nextid = 0;
            model.rootid = parendid;
            model.child = 0;
            model.depath = 0;

            if (parendid != 0)
            {
                var parent = (from a in ef.hx_td_department where a.department_id == parendid select a).SingleOrDefault();
                if (parent != null)
                {
                    model.rootid = parent.rootid;
                    model.parentpath = parent.parentpath;
                    model.depath = parent.depath + 1;
                }
            }

            ef.hx_td_department.Add(model);
            int id = ef.SaveChanges();
            if (id > 0)
            {
                //hx_td_department mode2 = new hx_td_department();
                model.parentpath = model.parentpath + "," + model.department_id + ",";
                model.department_id = model.department_id;
                DbEntityEntry entry = ef.Entry<hx_td_department>(model);
                //entry.State = EntityState.Unchanged;
                entry.Property("parentpath").IsModified = true;
                ef.SaveChanges();

                new SelectListByEF().ClearDepartmentInfo();
                return Content("{\"ret\":1,\"msg\":\"保存成功！\"}", "text/json");
            }
            else
            {
                return Content("{\"ret\":0,\"msg\":\"保存失败！\"}", "text/json");
            }

        }


        #endregion


    }
}