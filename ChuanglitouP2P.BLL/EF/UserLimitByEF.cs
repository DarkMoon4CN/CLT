using ChuanglitouP2P.DBUtility;
using ChuangLitouP2P.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace ChuanglitouP2P.BLL.EF
{
    /// <summary>
    /// 用户权限相关
    /// </summary>
    public class UserLimitByEF
    {
        chuangtouEntities ef = new chuangtouEntities();

        private string CacheKey = "AdminUserLimit_{0}";

        private string UserLeftListCacheKey = "AdminUserLeftLimit_{0}"; //用户左侧类别

        private string DepartmentCacheKey = "HX_DepartmnetLimitInfoCacheKey_{0}";

        private string getDepartmentCacheKey(int departmentId)
        {
            return string.Format(DepartmentCacheKey, departmentId);
        }

        public void ClearChildsDepartmentCacheKey(int departmentId)
        {
            ClearDepartmentCacheKey(departmentId);
            var list = ef.hx_td_department.Where(a => a.parentid == departmentId).OrderByDescending(p => (int)p.orderid);
            if (list != null && list.Count() > 0)
            {
                foreach (var item in list)
                {
                    ClearDepartmentCacheKey(item.department_id);
                    ClearChildsDepartmentCacheKey(item.department_id);
                }
            }
        }

        public void ClearDepartmentCacheKey(int departmentId)
        {
            ClearUserCacheByDepartmentId(departmentId);
            HttpRuntime.Cache.Remove(string.Format(DepartmentCacheKey, departmentId));
        }

        public void ClearUserCacheByDepartmentId(int departmentId)
        {
            var list = ef.hx_td_adminuser.Where(a => a.department_id == departmentId);
            if (list != null && list.Count() > 0)
            {
                foreach (var item in list)
                {
                    ClearUserCache(item.adminuserid);
                }
            }
        }

        /// <summary>
        /// 清空用户的缓存
        /// </summary>
        /// <param name="userId"></param>
        public void ClearUserCache(int userId)
        {
            HttpRuntime.Cache.Remove(string.Format(CacheKey, userId));
            HttpRuntime.Cache.Remove(string.Format(UserLeftListCacheKey, userId));
        }


        /// <summary>
        /// 用户左侧列表
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public Dictionary<int, List<hx_AdminLimitInfo>> GetUserLeftLimitInfo(int userId)
        {
            Dictionary<int, List<hx_AdminLimitInfo>> dic = new Dictionary<int, List<hx_AdminLimitInfo>>();
            string key = string.Format(UserLeftListCacheKey, userId);
            try
            {
                if (HttpRuntime.Cache[key] == null)
                {
                    //hx_td_adminuser user = (from u in ef.hx_td_adminuser where u.adminuserid == userId select u).SingleOrDefault();

                    StringBuilder sql = new StringBuilder();
                    sql.Append("WITH treeTB(id) as(");
                    sql.AppendFormat("select limitId from hx_DepUserLimit where adminUserId='{0}' ", userId);
                    sql.Append(" union all ");
                    sql.Append("select ParentId from hx_AdminLimitInfo inner join treeTB ON hx_AdminLimitInfo.id=treeTB.id)");
                    sql.Append(" select * from hx_AdminLimitInfo where level in (1,2,3) AND id in ( select * from treeTB );");
                    DataTable dt = DbHelperSQL.GET_DataTable_List(sql.ToString());

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string ids = "";
                        List<string> list_ids = new List<string>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (ids.Length > 0)
                            {
                                ids = ids + ",";
                            }
                            ids = ids + dr["id"].ToString();
                            list_ids.Add(dr["id"].ToString());
                        }

                        List<IGrouping<int, hx_AdminLimitInfo>> groupList = ef.hx_AdminLimitInfo.Where(a => a.isDel == 0 && list_ids.Contains(a.id.ToString()) && a.level < 4).GroupBy(m => m.ParentId).ToList();

                        dic = new Dictionary<int, List<hx_AdminLimitInfo>>();


                        if (groupList != null && groupList.Count() > 0)
                        {
                            foreach (var item in groupList)
                            {
                                dic.Add(item.Key, item.ToList());
                            }
                        }
                    }
                    HttpRuntime.Cache.Add(key, dic, null, DateTime.Now.AddMinutes(20), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                }
                else
                {
                    dic = HttpRuntime.Cache[key] as Dictionary<int, List<hx_AdminLimitInfo>>;
                }
            }
            catch
            {
                dic = null;
            }

            return dic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        //[OutputCache(Duration = 480, Location = OutputCacheLocation.ServerAndClient)]
        public List<V_DepUserLimitInfo> GetUserLimitInfoByUserId(int userId)
        {
            string key = string.Format(CacheKey, userId);
            List<V_DepUserLimitInfo> list = new List<V_DepUserLimitInfo>();
            try
            {

                if (HttpRuntime.Cache[key] == null)
                {
                    list = ef.V_DepUserLimitInfo.Where(p => p.isDel == 0 && p.adminUserId > 0 && p.adminUserId == userId).ToList();

                    HttpRuntime.Cache.Add(key, list, null, DateTime.Now.AddMinutes(20), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                }
                else
                {
                    list = HttpRuntime.Cache[key] as List<V_DepUserLimitInfo>;
                }
            }
            catch (Exception ex)
            {
                list = null;
            }

            return list;
        }

        /// <summary>
        /// 判断用户是否有访问权限
        /// </summary>
        /// <param name="userid">登录用户id</param>
        /// <param name="controllerName">controller名称</param>
        /// <param name="actionName">action名称</param>
        /// <returns>true:有权限，false:没有权限</returns>
        public bool CheckAdminLimit(int userid, string controllerName, string actionName)
        {
            var list = GetUserLimitInfoByUserId(userid);
            if (list == null || list.Count < 1)
            {
                return false;
            }
            foreach (V_DepUserLimitInfo item in list)
            {
                if (item.ControllerName.Trim().ToLower() == controllerName.Trim().ToLower() && item.ActionName.Trim().ToLower() == actionName.Trim().ToLower())
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// 部门权限
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public Dictionary<int, List<hx_AdminLimitInfo>> GetDepartmentLimit(int departmentId)
        {
            Dictionary<int, List<hx_AdminLimitInfo>> dic = new Dictionary<int, List<hx_AdminLimitInfo>>();
            string key = getDepartmentCacheKey(departmentId);

            try
            {
                if (HttpRuntime.Cache[key] == null)
                {
                    dic = getDepartmentLimitByDB(departmentId);
                    HttpRuntime.Cache.Add(key, dic, null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                }
                else
                {
                    dic = HttpRuntime.Cache[key] as Dictionary<int, List<hx_AdminLimitInfo>>;
                }
            }
            catch (Exception ex)
            {
                dic = null;
            }

            return dic;
        }
        /// <summary>
        /// 部门权限
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        private Dictionary<int, List<hx_AdminLimitInfo>> getDepartmentLimitByDB(int departmentId)
        {
            var result = (from item in ef.hx_AdminLimitInfo
                          group item by item.ParentId into g
                          select new
                          {
                              parentID = g.Key,
                              dataList = g.Select(c => c).ToList()
                          }).ToDictionary(c => c.parentID, d => d.dataList);
            return result;
            //Dictionary<int, List<hx_AdminLimitInfo>> dic = new Dictionary<int, List<hx_AdminLimitInfo>>();

            //if (departmentId > 0)
            //{

            //    //StringBuilder sql = new StringBuilder();
            //    //sql.Append("WITH treeTB(id) as(");
            //    //sql.AppendFormat("select LimitId from hx_DepUserLimit where LimitType=1 AND departmentId='{0}' ", departmentId);
            //    //sql.Append(" union ALL ");
            //    //sql.Append("select ParentId from hx_AdminLimitInfo inner join treeTB ON hx_AdminLimitInfo.id=treeTB.id)");
            //    //sql.Append("select * from hx_AdminLimitInfo where id in (");
            //    //sql.Append("select * from hx_AdminLimitInfo");
            //    //DataTable dt = null;// DbHelperSQL.GET_DataTable_List(sql.ToString());

            //    //var result = ef.Database.SqlQuery<hx_AdminLimitInfo>(sql.ToString());
            //    //if (result != null)
            //    //{
            //    //    var groupList = result.ToList().GroupBy(a => a.ParentId);

            //    //    if (groupList != null && groupList.Count() > 0)
            //    //    {
            //    //        foreach (var item in groupList)
            //    //        {
            //    //            dic.Add((int)item.Key, item.ToList());
            //    //        }
            //    //    }
            //    //}

                
            //}
            //else
            //{
            //    var groupList = ef.hx_AdminLimitInfo.GroupBy(m => m.ParentId).ToList();

            //    if (groupList != null && groupList.Count() > 0)
            //    {
            //        foreach (var item in groupList)
            //        {
            //            dic.Add((int)item.Key, item.ToList());
            //        }
            //    }
            //}

            //return dic;
        }



    }
}
