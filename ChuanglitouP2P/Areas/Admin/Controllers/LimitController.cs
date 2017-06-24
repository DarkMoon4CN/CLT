using ChuanglitouP2P.BLL.EF;
using ChuanglitouP2P.Common;
using ChuangLitouP2P.Models;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityFramework.Extensions;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using ChuanglitouP2P.DBUtility;
using System.Data;
using ChuanglitouP2P.Areas.Admin.Controllers.Filters;
using System.Data.SqlClient;

namespace ChuanglitouP2P.Areas.Admin.Controllers
{
    /// <summary>
    /// 权限相关
    /// </summary>
    public class LimitController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();

        // GET: Admin/Limit
        [AdminVaildate(false, true)]
        public ActionResult Index()
        {

            List<IGrouping<int, hx_AdminLimitInfo>> groupList = ef.hx_AdminLimitInfo.Where(a => a.isDel == 0).GroupBy(m => m.ParentId).ToList();

            Dictionary<int, List<hx_AdminLimitInfo>> dic = new Dictionary<int, List<hx_AdminLimitInfo>>();
            if (groupList != null && groupList.Count() > 0)
            {
                foreach (var item in groupList)
                {
                    dic.Add(item.Key, item.ToList());
                }
            }

            return View(dic);
            //IEnumerable<hx_AdminLimitInfo> list = ef.hx_AdminLimitInfo.Where(a => a.isDel == 0).OrderBy(a => a.SortId);
            //List<hx_AdminLimitInfo> sortList = new List<hx_AdminLimitInfo>();
            //var list0 = list.Where(a => a.ParentId == 0);
            //if (list0!=null && list0.Count()>0)
            //{
            //    List<IGrouping<int,hx_AdminLimitInfo>> sss = list.GroupBy(m => m.ParentId).ToList();
            //    foreach (IGrouping<int, hx_AdminLimitInfo> ss in sss)
            //    {
            //        var title= ss.Key;
            //        var pid= ss.ToList();
            //        var key =pid.Count();
            //    }
            //    foreach (hx_AdminLimitInfo item in list0)
            //    {
            //        sortList.Add(item);
            //        var list1 = list.Where(a => a.ParentId == item.id);
            //        if (list1!=null && list1.Count()>0)
            //        {
            //            foreach (var item1 in list1)
            //            {
            //                sortList.Add(item1);
            //                var list2 = list.Where(a => a.ParentId == item1.id);
            //                if (list2 != null && list2.Count() > 0)
            //                {
            //                    foreach (var item2 in list2)
            //                    {
            //                        sortList.Add(item2);
            //                        var list3 = list.Where(a => a.ParentId == item2.id);
            //                        if (list3 != null && list3.Count() > 0)
            //                        {
            //                            foreach (var item3 in list3)
            //                            {
            //                                sortList.Add(item3);
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}


            //return View(sortList);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult Remove(int id)
        {
            if (id < 1)
            {
                return Content(StringAlert.Alert("参数错误", "/admin/Limit/Index"), "text/html");
            }
            var result = ef.hx_AdminLimitInfo.Where(a => a.id == id).Update(a => new hx_AdminLimitInfo { isDel = 1, lastTime = DateTime.Now, lastOper = Utils.GetAdmUserID().ToString() });

            if (result > 0)
            {
                return Content(StringAlert.Alert("操作成功", "/admin/Limit/Index"), "text/html");
            }
            else
            {
                return Content(StringAlert.Alert("操作失败", "/admin/Limit/Index"), "text/html");
            }
        }

        #region 用户权限

        /// <summary>
        /// 用户权限
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult UserLimit(int userid)
        {
            Dictionary<int, List<hx_AdminLimitInfo>> dic = new Dictionary<int, List<hx_AdminLimitInfo>>();
            hx_td_adminuser user = (from u in ef.hx_td_adminuser where u.adminuserid == userid select u).SingleOrDefault();
            if (user == null || user.adminuserid < 1)
            {
                return Content(StringAlert.Alert("没有找到相关的用户信息！"), "text/html");
            }
            ////var sql = string.Format("", user.department_id);
            //StringBuilder sql = new StringBuilder();
            //sql.Append("WITH treeTB(id) as(");
            //sql.AppendFormat("select limitId from hx_DepUserLimit where departmentId='{0}' ", user.department_id);
            //sql.Append(" union all ");
            //sql.Append("select ParentId from hx_AdminLimitInfo inner join treeTB ON hx_AdminLimitInfo.id=treeTB.id)");
            //sql.Append("select * from treeTB;");
            //DataTable dt = DbHelperSQL.GET_DataTable_List(sql.ToString());

            //if (dt != null && dt.Rows.Count > 0)
            //{
            //    string ids = "";
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        if (ids.Length>0)
            //        {
            //            ids = ids + ",";
            //        }
            //        ids = ids + dr["id"].ToString();
            //    }

            //    List<IGrouping<int, hx_AdminLimitInfo>> groupList = ef.hx_AdminLimitInfo.Where(a => a.isDel == 0 && ids.Contains(a.id.ToString())).GroupBy(m => m.ParentId).ToList();
            //    dic = new Dictionary<int, List<hx_AdminLimitInfo>>();


            //    if (groupList != null && groupList.Count() > 0)
            //    {
            //        foreach (var item in groupList)
            //        {
            //            dic.Add(item.Key, item.ToList());
            //        }
            //    }
            //}
            dic = new ChuanglitouP2P.BLL.EF.UserLimitByEF().GetDepartmentLimit((int)user.department_id);


            ViewBag.userid = userid;
            ViewBag.department_id = user.department_id;

            return View(dic);
        }

        /// <summary>
        /// 获取部门的权限
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ActionResult GetLimitByUserId(int userid)
        {
            var jsons = "";
            List<hx_DepUserLimit1> list = (from a in ef.hx_DepUserLimit1 where a.limitType == 2 && a.adminUserId == userid select a).ToList();
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (!string.IsNullOrEmpty(jsons))
                    {
                        jsons = jsons + ",";
                    }
                    jsons = jsons + "{\"lid\":" + item.limitId + "}";
                }
            }

            return Content(string.Format("[{0}]", jsons), "text/json");
        }
        /// <summary>
        /// 保存部门权限
        /// </summary>
        /// <param name="departmentid"></param>
        /// <param name="limitids"></param>
        /// <returns></returns>
        public ActionResult SaveUserLimit(int userid, int departmentid, string limitids)
        {
            var json = "{\"ret\":-1,\"msg\":\"参数错误\"}";
            if (userid > 0)
            {
                var result = ef.hx_DepUserLimit1.Where(a => a.adminUserId == userid).Delete();
                var errorNum = 0;
                if (!string.IsNullOrEmpty(limitids))
                {
                    var arrLimitId = limitids.Split('|');
                    foreach (var item in arrLimitId)
                    {
                        hx_DepUserLimit1 l = new hx_DepUserLimit1();
                        l.adminUserId = Utils.GetAdmUserID();
                        l.createTime = DateTime.Now;
                        l.departmentId = departmentid;
                        l.adminUserId = userid;
                        l.limitId = int.Parse(item);
                        l.limitType = 2;

                        ef.hx_DepUserLimit1.Add(l);
                        int id = ef.SaveChanges();
                        if (id < 1)
                        {
                            errorNum += 1;
                        }
                    }
                }
                if (errorNum > 0)
                {
                    json = "{\"ret\":0,\"msg\":\"保存失败\"}";
                }
                else
                {
                    new UserLimitByEF().ClearUserCache(userid);
                    json = "{\"ret\":1,\"msg\":\"保存成功\"}";
                }
            }

            return Content(json, "text/json");
        }


        public ActionResult GetUserLeftLimit()
        {
            var userid = Utils.GetAdmUserID();
            Dictionary<int, List<hx_AdminLimitInfo>> dic = new Dictionary<int, List<hx_AdminLimitInfo>>();
            hx_td_adminuser user = (from u in ef.hx_td_adminuser where u.adminuserid == userid select u).SingleOrDefault();

            //var sql = string.Format("", user.department_id);
            StringBuilder sql = new StringBuilder();
            sql.Append("WITH treeTB(id) as(");
            sql.AppendFormat("select limitId from hx_DepUserLimit where departmentId='{0}' ", user.department_id);
            sql.Append(" union all ");
            sql.Append("select ParentId from hx_AdminLimitInfo inner join treeTB ON hx_AdminLimitInfo.id=treeTB.id)");
            sql.Append("select * from treeTB;");
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql.ToString());

            if (dt != null && dt.Rows.Count > 0)
            {
                string ids = "";
                foreach (DataRow dr in dt.Rows)
                {
                    if (ids.Length > 0)
                    {
                        ids = ids + ",";
                    }
                    ids = ids + dr["id"].ToString();
                }

                List<IGrouping<int, hx_AdminLimitInfo>> groupList = ef.hx_AdminLimitInfo.Where(a => a.isDel == 0 && ids.Contains(a.id.ToString()) && a.level < 4).GroupBy(m => m.ParentId).ToList();
                dic = new Dictionary<int, List<hx_AdminLimitInfo>>();


                if (groupList != null && groupList.Count() > 0)
                {
                    foreach (var item in groupList)
                    {
                        dic.Add(item.Key, item.ToList());
                    }
                }
            }

            return View(dic);
        }

        #endregion

        #region 部门权限

        /// <summary>
        /// 部门权限
        /// </summary>
        /// <param name="departmentid"></param>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult DepartmentLimit(int departmentid)
        {
            //List<IGrouping<int, V_DepUserLimitInfo>> groupList = ef.V_DepUserLimitInfo.Where(a => a.isDel == 0 && (a.departmentId==departmentid || a.departmentId==null)).GroupBy(m => m.ParentId).ToList();
            //Dictionary<int, List<V_DepUserLimitInfo>> dic = new Dictionary<int, List<V_DepUserLimitInfo>>();

            //List<IGrouping<int, hx_AdminLimitInfo>> groupList = ef.hx_AdminLimitInfo.Where(a => a.isDel == 0).GroupBy(m => m.ParentId).ToList();
            //Dictionary<int, List<hx_AdminLimitInfo>> dic = new Dictionary<int, List<hx_AdminLimitInfo>>();


            //if (groupList != null && groupList.Count() > 0)
            //{
            //    foreach (var item in groupList)
            //    {
            //        dic.Add(item.Key, item.ToList());
            //    }
            //}


            hx_td_department dep = (from a in ef.hx_td_department where a.department_id == departmentid select a).SingleOrDefault();
            if (dep == null || dep.department_id < 1)
            {
                return Content(StringAlert.Alert("没有找到对应的部门信息！"), "text/html");
            }

            Dictionary<int, List<hx_AdminLimitInfo>> dic = new ChuanglitouP2P.BLL.EF.UserLimitByEF().GetDepartmentLimit((int)dep.parentid);

            ViewBag.departmentid = departmentid;

            return View(dic);
        }

        /// <summary>
        /// 获取部门的权限
        /// </summary>
        /// <param name="departmentid"></param>
        /// <returns></returns>
        public ActionResult GetLimitByDepartmentId(int departmentid)
        {
            var jsons = "";
            List<int?> list = (from a in ef.hx_DepUserLimit1 where a.limitType == 1 && a.departmentId == departmentid select a.limitId).ToList();
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (!string.IsNullOrEmpty(jsons))
                    {
                        jsons = jsons + ",";
                    }
                    jsons = jsons + "{\"lid\":" + item + "}";
                }
            }

            return Content(string.Format("[{0}]", jsons), "text/json");
        }

        /// <summary>
        /// 保存部门权限
        /// </summary>
        /// <param name="departmentid"></param>
        /// <param name="limitids"></param>
        /// <returns></returns>
        public ActionResult SaveDepartmentLimit(int departmentid, string limitids)
        {
            var json = "{\"ret\":-1,\"msg\":\"参数错误\"}";
            if (departmentid > 0)
            {
                int uid = Utils.GetAdmUserID();
                var arrLimitId = limitids.Split('|').ToList();

                var query = (from item in ef.hx_td_department
                             where item.parentpath.StartsWith(ef.hx_td_department.Where(c => c.department_id == departmentid).FirstOrDefault().parentpath)
                             select item.department_id);

                List<int> childDeps = query.ToList();

                //List<int> childDeps = (from item in ef.hx_td_department
                //                       where item.parentpath.StartsWith(ef.hx_td_department.Where(c => c.department_id == departmentid).First().parentpath)
                //                       select item.department_id).ToList();

                //ef.hx_DepUserLimit1.Where(c => childDeps.Contains(Convert.ToInt32(c.limitId)) && !arrLimitId.Contains(c.limitId.ToString())).Delete();

                var query1 = ef.hx_DepUserLimit1.Where(c => childDeps.Contains((int)c.departmentId) && !arrLimitId.Contains(c.limitId.ToString()));
                query1.Delete(); 


                List<hx_DepUserLimit1> limits = new List<hx_DepUserLimit1>();
                List<string> litIDs = arrLimitId.Where(c => !ef.hx_DepUserLimit1.Select(d => d.limitId.ToString()).Contains(c)).ToList();
                foreach (var item in litIDs)
                {
                    hx_DepUserLimit1 l = new hx_DepUserLimit1();
                    //l.adminUserId = uid;//Utils.GetAdmUserID();
                    l.createTime = DateTime.Now;
                    l.departmentId = departmentid;
                    l.limitId = int.Parse(item);
                    l.limitType = 1;
                    limits.Add(l);
                }
                ef.hx_DepUserLimit1.AddRange(limits);
                int changeCount = ef.SaveChanges();
                if (changeCount != litIDs.Count)
                {
                    json = "{\"ret\":0,\"msg\":\"保存失败\"}";
                }
                else
                {
                    new UserLimitByEF().ClearChildsDepartmentCacheKey(departmentid);
                    ClearChildsDepartmentLimit(departmentid);
                    json = "{\"ret\":1,\"msg\":\"保存成功\"}";
                }
                //var result = ef.hx_DepUserLimit1.Where(a => a.departmentId == departmentid).Delete();
                //var errorNum = 0;
                //if (!string.IsNullOrEmpty(limitids))
                //{
                //    var arrLimitId = limitids.Split('|');
                //    foreach (var item in arrLimitId)
                //    {
                //        hx_DepUserLimit1 l = new hx_DepUserLimit1();
                //        l.adminUserId = Utils.GetAdmUserID();
                //        l.createTime = DateTime.Now;
                //        l.departmentId = departmentid;
                //        l.limitId = int.Parse(item);
                //        l.limitType = 1;

                //        ef.hx_DepUserLimit1.Add(l);
                //        int id = ef.SaveChanges();
                //        if (id < 1)
                //        {
                //            errorNum += 1;
                //        }
                //    }
                //}
                //if (errorNum > 0)
                //{
                //    json = "{\"ret\":0,\"msg\":\"保存失败\"}";
                //}
                //else
                //{
                //    new UserLimitByEF().ClearChildsDepartmentCacheKey(departmentid);
                //    ClearChildsDepartmentLimit(departmentid);
                //    json = "{\"ret\":1,\"msg\":\"保存成功\"}";
                //}
            }

            return Content(json, "text/json");
        }

        private void ClearChildsDepartmentLimit(int departmentId)
        {
            var list = ef.hx_td_department.Where(a => a.parentid == departmentId).OrderByDescending(a => (int)a.orderid);
            if (list != null && list.Count() > 0)
            {
                foreach (var item in list)
                {

                    string sql = string.Format(@"delete from hx_DepUserLimit where  departmentId ='{0}' 
            and limitId not in (select limitId from hx_DepUserLimit where limittype=1 and departmentId ='{1}')", item.department_id, departmentId);

                    DbHelperSQL.ExecuteSql(sql);
                    ClearChildsDepartmentLimit(item.department_id);
                }
            }
        }

        #endregion

        #region 编辑

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult Editor(int id)
        {
            var item = (from a in ef.hx_AdminLimitInfo where a.id == id select a).FirstOrDefault();

            ViewBag.ParentSelect = new SelectListByEF().GetLimitDropDownList(false, "0", "添加为根栏目");

            return View(item);
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Editor(hx_AdminLimitInfo t)
        {
            t = (hx_AdminLimitInfo)Common.Utils.ValidateModelClass(t);
            t.ControllerName = Utils.CheckSQL(DNTRequest.GetString("ConName"));
            t.ActionName = Utils.CheckSQL(DNTRequest.GetString("ActName"));
            string[] proNames;

            proNames = new string[] { "title", "ControllerName", "ActionName", "SortId", "lastOper", "lastTime" };
            t.lastOper = Utils.GetAdmUserID().ToString();
            t.lastTime = DateTime.Now;
            DbEntityEntry entry = ef.Entry<hx_AdminLimitInfo>(t);
            entry.State = EntityState.Unchanged;

            foreach (string ProName in proNames)
            {
                entry.Property(ProName).IsModified = true;
            }
            int result = ef.SaveChanges();
            if (result > 0)
            {
                return Content(StringAlert.Alert("操作成功", "/admin/Limit/Index"), "text/html");
            }
            else
            {
                return Content(StringAlert.Alert("操作失败"), "text/html");
            }
        }

        #endregion

        #region 新增

        /// <summary>
        /// 添加权限信息
        /// </summary>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult Add()
        {
            ViewBag.ParentSelect = new SelectListByEF().GetLimitDropDownList(false, "0", "添加为根栏目");

            return View();
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Add(hx_AdminLimitInfo t)
        {
            t = (hx_AdminLimitInfo)Common.Utils.ValidateModelClass(t);
            t.isDel = 0;
            t.lastOper = Utils.GetAdmUserID().ToString();
            t.lastTime = DateTime.Now;
            t.CreatTime = DateTime.Now;
            ef.hx_AdminLimitInfo.Add(t);
            int id = ef.SaveChanges();
            if (id > 0)
            {
                //return Content(StringAlert.Alert("操作成功"), "text/html");
                return Content(StringAlert.Alert("操作成功", "/admin/Limit/Index"), "text/html");
            }
            else
            {
                return Content(StringAlert.Alert("操作失败"), "text/html");
            }
        }

        #endregion
    }
}