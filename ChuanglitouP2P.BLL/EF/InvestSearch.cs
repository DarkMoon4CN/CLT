using ChuanglitouP2P.Common;
using ChuanglitouP2P.DBUtility;
using ChuangLitouP2P.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ChuanglitouP2P.BLL.EF
{
    /// <summary>
    /// 投资 查询项目
    /// </summary>
    public class InvestSearch
    {
        chuangtouEntities ef = new chuangtouEntities();

        /// <summary>
        /// 从缓存获取投资的查询条件
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<hx_investsearch> GetInvestSearchByType(EnumInvestSearch type)
        {
            var list = InvestSearchCache();

            if (list==null || list.Count<1)
            {
                return null;
            }
            var list_type = list.FindAll(a => a.investtype == (int)type);

            return list_type;
        }

        /// <summary>
        /// 获取根据主键获取内容信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public hx_investsearch GetModelById(int id)
        {
            if (id<1)
            {
                return null;
            }
            var list = InvestSearchCache();
            if (list == null || list.Count < 1)
            {
                return null;
            }
            var item = list.Find(a => a.investsearchid == id);

            return item;
        }

        /// <summary>
        /// 从缓存中获取投资筛选条件信息
        /// </summary>
        /// <returns></returns>
        public List<hx_investsearch> InvestSearchCache()
        {
            List<hx_investsearch> list = new List<hx_investsearch>();
            string key = "investSearch_CacheData";
            try
            {
                if (HttpRuntime.Cache[key] == null)
                {
                    list = (from a in ef.hx_investsearch select a).OrderBy(a => a.investtype).ToList();
                    HttpRuntime.Cache.Add(key, list, null, DateTime.Now.AddMinutes(20), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                }
                else
                {
                    list = HttpRuntime.Cache[key] as List<hx_investsearch>;
                }
            }
            catch (Exception ex)
            {
                list = null;
            }
            return list;
        }

        /// <summary>
        /// 项目类型
        /// </summary>
        /// <returns></returns>
        public List<hx_Project_type> ProjectTypeCache()
        {
            List<hx_Project_type> list = new List<hx_Project_type>();
            string key = "Project_type_CacheData";
            try
            {
                if (HttpRuntime.Cache[key] == null)
                {
                    list = (from a in ef.hx_Project_type select a).OrderBy(a => a.project_type_id).ToList();
                    HttpRuntime.Cache.Add(key, list, null, DateTime.Now.AddMinutes(20), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                }
                else
                {
                    list = HttpRuntime.Cache[key] as List<hx_Project_type>;
                }
            }
            catch (Exception ex)
            {
                list = null;
            }
            return list;
        }
    }
}
