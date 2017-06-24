using ChuangLitouP2P.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace ChuanglitouP2P.BLL.EF
{
    public class SelectListByEF
    {
        chuangtouEntities ef = new chuangtouEntities();


        #region 获取部门下拉菜单 + List<SelectListItem> PDropDownList() 
        /// <summary>
        /// 获取部门下拉菜单
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 480, Location = OutputCacheLocation.ServerAndClient)]
        public List<SelectListItem> PDropDownList()
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            string key = "deparDropDownList";
            List<hx_td_department> dep = new List<hx_td_department>();
            try
            {
                if (HttpRuntime.Cache[key] == null)
                {
                    dep = ef.hx_td_department.Where(p => p.department_id > 0).OrderBy(p => p.parentid).OrderByDescending(p => (int)p.orderid).ToList();
                    HttpRuntime.Cache.Add(key, dep, null, DateTime.Now.AddMinutes(20), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                }
                else
                {
                    dep = HttpRuntime.Cache[key] as List<hx_td_department>;
                }
            }
            catch
            {
                dep = null;
            }

            foreach (hx_td_department de in dep.Where(n => n.parentid == 0))
            {
                string parentName = de.department_name;
                string parentId = de.department_id.ToString();
                selectList.Add(new SelectListItem { Text = "├-" + parentName, Value = parentId });
                string pid = parentId;
                string tag = "　|- ";
                ChildrenList(pid, tag, selectList, dep);

            }

            return selectList;
        }
        #endregion


        #region 递归部门子类别 + ChildrenList(string pid, string tag, List<SelectListItem> selectList, List<hx_td_department> dep1)
        /// <summary>
        /// 递归部门子类别
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="tag"></param>
        /// <param name="selectList"></param>
        /// <param name="dep1"></param>
        [OutputCache(Duration = 480, Location = OutputCacheLocation.ServerAndClient)]
        private void ChildrenList(string pid, string tag, List<SelectListItem> selectList, List<hx_td_department> dep1)
        {
            //List<hx_td_department> child = dep1.Where(c => c.parentid > 0 && c.parentid.ToString() == pid).ToList();

            int intpid = int.Parse(pid);
            List<hx_td_department> child = (from a in dep1 where a.parentid == intpid && a.parentid > 0 select a).ToList();

            if (child.Count > 0)
            {
                foreach (var item in child)
                {
                    string clname = tag + item.department_name;
                    string clid = item.department_id.ToString();

                    selectList.Add(new SelectListItem { Text = clname, Value = clid });
                    if (item.depath > 1)
                    {
                        string pid2 = clid;
                        string tag2 = tag + " -- ";
                        ChildrenList(pid, tag, selectList, dep1);
                    }
                }

            }
        }
        #endregion


        #region 获取菜单下拉列表 +List<SelectListItem> GetMuenDropDownList() 
        /// <summary>
        /// 获取菜单下拉列表
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 480, Location = OutputCacheLocation.ServerAndClient)]
        public List<SelectListItem> GetMuenDropDownList()
        {
            List<SelectListItem> selectList = new List<SelectListItem>();

            string key = "MuenDropDownList";
            List<hx_td_menu> dep = new List<hx_td_menu>();
            try
            {
                if (HttpRuntime.Cache[key] == null)
                {
                    dep = ef.hx_td_menu.Where(p => p.menu_id > 0).OrderBy(p => p.parentid).ToList();
                    HttpRuntime.Cache.Add(key, dep, null, DateTime.Now.AddMinutes(20), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                }
                else
                {
                    dep = HttpRuntime.Cache[key] as List<hx_td_menu>;

                }
            }
            catch
            {
                dep = null;
            }
            foreach (hx_td_menu de in dep.Where(n => n.parentid == 0))
            {
                string parentName = de.menu_name;
                string parentId = de.menu_id.ToString();
                selectList.Add(new SelectListItem { Text = "├-" + parentName, Value = parentId });
                string pid = parentId;
                string tag = "　|- ";
                GetChildrenMuenList(pid, tag, selectList, dep);

            }

            return selectList;

        }
        #endregion

        #region 获取菜单子类 + GetChildrenMuenList(string pid, string tag, List<SelectListItem> selectList, List<hx_td_menu> dep1)
        /// <summary>
        /// 获取菜单子类 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="tag"></param>
        /// <param name="selectList"></param>
        /// <param name="dep1"></param>
        [OutputCache(Duration = 480, Location = OutputCacheLocation.ServerAndClient)]
        private void GetChildrenMuenList(string pid, string tag, List<SelectListItem> selectList, List<hx_td_menu> dep1)
        {
            //List<hx_td_department> child = dep1.Where(c => c.parentid > 0 && c.parentid.ToString() == pid).ToList();

            int intpid = int.Parse(pid);
            List<hx_td_menu> child = (from a in dep1 where a.parentid == intpid && a.parentid > 0 select a).ToList();

            if (child.Count > 0)
            {
                foreach (var item in child)
                {
                    string clname = tag + item.menu_name;
                    string clid = item.menu_id.ToString();

                    selectList.Add(new SelectListItem { Text = clname, Value = clid });
                    if (item.depath > 1)
                    {
                        string pid2 = clid;
                        string tag2 = tag + " -- ";
                        GetChildrenMuenList(pid, tag, selectList, dep1);
                    }
                }

            }
        }


        #endregion

        #region 获取银行下拉列表 + GetBandDropDownList()

        /// <summary>
        /// 获取银行下拉列表
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 480, Location = OutputCacheLocation.ServerAndClient)]
        public List<SelectListItem> GetBandDropDownList(string defaultKey = "", string defaultVal = "")
        {

            List<SelectListItem> selectList = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(defaultKey) || !string.IsNullOrEmpty(defaultVal))
            {
                selectList.Add(new SelectListItem { Text = defaultVal, Value = defaultKey });
            }
            string key = "BankDropDownList";
            List<hx_td_Bank> dep = new List<hx_td_Bank>();
            try
            {


                if (HttpRuntime.Cache[key] == null)
                {
                    dep = ef.hx_td_Bank.Where(p => p.Bankid > 0).ToList();
                    HttpRuntime.Cache.Add(key, dep, null, DateTime.Now.AddMinutes(20), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                }
                else
                {
                    dep = HttpRuntime.Cache[key] as List<hx_td_Bank>;
                }

                foreach (hx_td_Bank de in dep)
                {
                    string parentName = de.BankName;
                    string parentId = de.OpenBankId.ToString();
                    selectList.Add(new SelectListItem { Text = parentName, Value = parentId });

                }
            }
            catch (Exception ex)
            {
                dep = null;
            }


            return selectList;
        }

        #endregion

        #region 获取用户权限下拉列表 + GetLimitDropDownList()

        /// <summary>
        /// 获取用户权限下拉列表
        /// </summary>
        /// <param name="ParentId">父节点id</param>
        /// <param name="includeButton">是否包含按钮</param>
        /// <param name="defaultKey"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        public List<SelectListItem> GetLimitDropDownList(bool includeButton, string defaultKey = "", string defaultVal = "")
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(defaultKey) || !string.IsNullOrEmpty(defaultVal))
            {
                selectList.Add(new SelectListItem { Text = defaultVal, Value = defaultKey });
            }
            string key = "LimitDropDownList";
            List<hx_AdminLimitInfo> list = new List<hx_AdminLimitInfo>();

            try
            {
                ///TODO 此处为加清空缓存的方法，加上后需要将注销的缓存恢复
                //if (HttpRuntime.Cache[key] == null)
                //{

                if (includeButton)
                {   //全部
                    list = ef.hx_AdminLimitInfo.Where(p => p.isDel == 0 ).ToList();
                }
                else
                {
                    list = ef.hx_AdminLimitInfo.Where(p => p.isDel == 0 && p.level < 4).ToList();
                }

                //HttpRuntime.Cache.Add(key, list, null, DateTime.Now.AddMinutes(20), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                //}
                //else
                //{
                //    list = HttpRuntime.Cache[key] as List<hx_AdminLimitInfo>;
                //}

                List< hx_AdminLimitInfo> list_1 = list != null && list.Count>0 ? list.FindAll(a => a.ParentId == 0) : null;
                if (list_1 != null && list_1.Count > 0)
                {
                    foreach (hx_AdminLimitInfo item in list_1)
                    {
                        string _text = item.title;
                        string _value = item.id.ToString();
                        selectList.Add(new SelectListItem { Text = _text, Value = _value });
                        var _tag = "|-";
                        //第二层
                        var list_2 = list.FindAll(a => a.ParentId == item.id).ToList();
                        if (list_2!=null && list_2.Count>0)
                        {
                            foreach (hx_AdminLimitInfo item2 in list_2)
                            {
                                _text = _tag + item2.title;
                                _value = item2.id.ToString();
                                selectList.Add(new SelectListItem { Text = _text, Value = _value });
                               var _tag2 = "|---";
                                //第三层
                                var list_3 = list.FindAll(a => a.ParentId == item2.id).ToList();
                                if (list_3 != null && list_3.Count > 0)
                                {
                                    foreach (hx_AdminLimitInfo item3 in list_3)
                                    {
                                        _text = _tag2 + item3.title;
                                        _value = item3.id.ToString();
                                        selectList.Add(new SelectListItem { Text = _text, Value = _value });
                                      var  _tag3 = "|-----";
                                        //第四层
                                        var list_4 = list.FindAll(a => a.ParentId == item3.id).ToList();
                                        if (list_4 != null && list_4.Count > 0)
                                        {
                                            foreach (hx_AdminLimitInfo item4 in list_4)
                                            {
                                                _text = _tag3 + item4.title;
                                                _value = item4.id.ToString();
                                                selectList.Add(new SelectListItem { Text = _text, Value = _value });
                                                
                                            }
                                        }

                                    }
                                }

                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                list = null;
            }


            return selectList;

        }

        #endregion

        #region 获取部门信息 + GetDepartmentDropDownList

        public void ClearDepartmentInfo()
        {
            HttpRuntime.Cache.Remove("DepartmentDropDownList");
        }
        //[OutputCache(Duration = 480, Location = OutputCacheLocation.ServerAndClient)]
        public List<SelectListItem> GetDepartmentDropDownList(int thisid, string defaultKey = "", string defaultValue = "")
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(defaultKey) || !string.IsNullOrEmpty(defaultValue))
            {
                selectList.Add(new SelectListItem { Text = defaultValue, Value = defaultKey });
            }
            string key = "DepartmentDropDownList";
            List<hx_td_department> list = new List<hx_td_department>();

            try
            {
                if (HttpRuntime.Cache[key] == null)
                {//全部
                    list = ef.hx_td_department.Where(p => p.department_id >0).OrderByDescending(p => (int)p.orderid).ToList();

                    HttpRuntime.Cache.Add(key, list, null, DateTime.Now.AddMinutes(20), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                }
                else
                {
                    list = HttpRuntime.Cache[key] as List<hx_td_department>;
                }
                
                if (list != null && list.Count > 0)
                {
                    var list0 = list.FindAll(a => a.parentid == 0);
                    if (list0!=null && list0.Count>0)
                    {
                        foreach (var item in list0)
                        {
                            if (item.department_id== thisid)
                            {
                                continue;
                            }
                            SelectListItem sli = new SelectListItem();
                            sli.Value = item.department_id.ToString();
                            sli.Text = item.department_name;
                            selectList.Add(sli);
                            var _tag = "|-";
                            var _list = GetChildList(thisid,item.department_id, list, _tag);
                            if (_list != null && _list.Count > 0)
                            {
                                selectList.AddRange(_list);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                list = null;
            }


            return selectList;
        }

       private List<SelectListItem> GetChildList(int thisid,int parentId,List<hx_td_department> list,string tag)
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            if (list!=null && list.Count>0)
            {
                var listChild = list.FindAll(a => a.parentid == parentId);
                if (listChild!=null && listChild.Count>0)
                {
                    foreach (var item in listChild)
                    {
                        if (item.department_id == thisid)
                        {
                            continue;
                        }
                        SelectListItem sli = new SelectListItem();
                        sli.Value = item.department_id.ToString();
                        sli.Text = tag + item.department_name;
                        selectList.Add(sli);
                        var _tag = tag + "--";
                        var _list = GetChildList(thisid,item.department_id, list, _tag);
                        if (_list!=null && _list.Count>0)
                        {
                            selectList.AddRange(_list);
                        }
                    }
                }
            }

            return selectList;
        }

        #endregion


        #region 奖励类型 + GetScheduleDropDownList
        /// <summary>
        /// 奖励类型
        /// </summary>
        /// <param name="defaultKey"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        public List<SelectListItem> GetScheduleDropDownList(string defaultKey="",string defaultVal="")
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(defaultKey) || !string.IsNullOrEmpty(defaultVal))
            {
                selectList.Add(new SelectListItem { Text = defaultVal, Value = defaultKey });
            }
            string key = "ScheduleDropDownList";
            List<hx_RewardType> list = new List<hx_RewardType>();
            try
            {
                if (HttpRuntime.Cache[key] == null)
                {
                    list = (from a in ef.hx_RewardType select a).OrderBy(a => a.RewTypeID).ToList();
                    HttpRuntime.Cache.Add(key, list, null, DateTime.Now.AddMinutes(20), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                }
                else
                {
                    list = HttpRuntime.Cache[key] as List<hx_RewardType>;
                }
                if (list!=null && list.Count>0)
                {
                    foreach (hx_RewardType item in list)
                    {
                        selectList.Add(new SelectListItem { Text = item.RewTypeName, Value = item.RewTypeID.ToString() });
                    }
                }

            }
            catch (Exception ex)
            {

                throw;
            }

            return selectList;
        }

        #endregion


        #region 获取活动类型下拉框
        /// <summary>
        /// 获取活动类型下拉框
        /// </summary>
        /// <param name="defaultKey"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        public List<SelectListItem> GetActivityType_DropDownList(string defaultKey = "", string defaultVal = "")
        {
            List<SelectListItem> selectList = new List<SelectListItem>();

            if (!string.IsNullOrEmpty(defaultKey) || !string.IsNullOrEmpty(defaultVal))
            {
                selectList.Add(new SelectListItem { Text = defaultVal, Value = defaultKey });
            }
            string key = "GetActivityType_DropDownList";
            List<hx_ActivityType> list = new List<hx_ActivityType>();
            try
            {
                if (HttpRuntime.Cache[key] == null)
                {
                    list = (from a in ef.hx_ActivityType select a).OrderBy(a => a.ActTypeId).ToList();
                    HttpRuntime.Cache.Add(key, list, null, DateTime.Now.AddMinutes(20), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                }
                else
                {
                    list = HttpRuntime.Cache[key] as List<hx_ActivityType>;
                }
                if (list != null && list.Count > 0)
                {
                    foreach (hx_ActivityType item in list)
                    {
                        selectList.Add(new SelectListItem { Text = item.ActName, Value = item.ActTypeId.ToString() });
                    }
                }

            }
            catch (Exception ex)
            {

                throw;
            }


            return selectList;

        } 
        #endregion
    }
}

