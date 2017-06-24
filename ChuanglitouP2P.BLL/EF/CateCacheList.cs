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
    public class CateCacheList
    {
        chuangtouEntities ef = new chuangtouEntities();





        #region 获取活动奖励类型 + List<hx_RewardType> GetCacheRewardType()
        /// <summary>
        /// 获取活动奖励类型  
        /// </summary>
        /// <returns></returns>
       // [OutputCache(Duration = 480, Location = OutputCacheLocation.ServerAndClient)]
        public List<hx_RewardType> GetCacheRewardType()
        {
            List<hx_RewardType> RewardList = new List<hx_RewardType>();
            string key = "RewardTypelist";
            try
            {
                if (HttpRuntime.Cache[key] == null)
                {
                    RewardList = ef.hx_RewardType.Where(p => p.RewTypeID > 0).OrderBy(p => p.RewTypeID).ToList();
                    HttpRuntime.Cache.Add(key, RewardList, null, DateTime.Now.AddMinutes(20), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                }
                else
                {
                    RewardList = HttpRuntime.Cache[key] as List<hx_RewardType>;
                }
            }
            catch
            {
                RewardList = null;
            }
            return RewardList;
        } 
        #endregion






    }
}
