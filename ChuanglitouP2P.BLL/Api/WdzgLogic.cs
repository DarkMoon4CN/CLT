using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChuangLiTou.Core.Entities.P2Peye;
using ChuanglitouP2P.Common.Util;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.DAL.Api;
namespace ChuanglitouP2P.BLL.Api
{

    public class WdzgLogic
    {
        private readonly WdzgDal _dal = new WdzgDal();


        /// <summary>
        /// 获取指定标信息
        /// </summary>
        /// <param name="id">标的编号</param> 
        /// <returns></returns>
        public dynamic SelectTargetByIdOrDate(string id, string date)
        {

            #region 数据验证
            var pId = ConvertHelper.ParseValue(id, 0);
            if (pId < 0) return null;
            #endregion 


            #region 验证缓存数据

            var cacheKey = CacheHelper.GetCacheKeyByParam(new IComparable[] { id ,ConvertHelper.ParseValue(date,DateTime.Now)}).ToLower();
            var objEntity = CacheHelper.GetCache(cacheKey);

            if (objEntity == null)
            {
                var item = _dal.SelectTargetByIdOrDate(pId, date);
                CacheHelper.SetCache(cacheKey, item);
                return item;
            }

            #endregion

            return objEntity;


             
        }
    }
}
