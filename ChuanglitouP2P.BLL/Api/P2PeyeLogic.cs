using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChuangLiTou.Core.Entities.P2Peye;
using ChuangLiTou.Core.Entities.ttnz;
using ChuanglitouP2P.Common.Util;
using ChuanglitouP2P.DAL.Api;
using ChuanglitouP2P.Common;
namespace ChuanglitouP2P.BLL.Api
{
    public class P2PeyeLogic
    {
        private readonly P2PeyeDal _dal = new P2PeyeDal();
        /// <summary>
        /// 获取借款列表.
        /// </summary>
        /// <param name="status">	
        /// 标的状态:0.正在投标中的借款标;1.已完成(包括还款中和已完成的借款标). 
        /// 状态为1是对应平台满标字段的值检索,状态为0就以平台发标时间字段检索.</param>
        /// <param name="time_from">始时间如:2014-05-09 06:10:00,</param>
        /// <param name="time_to">截止时间如:2014-05-09 06:10:00,</param>
        /// <param name="page_size">每页记录条数.</param>
        /// <param name="page_index">请求的页码.</param>
        /// <returns>List&lt;LoanEntity&gt;.</returns>
        public Pagination<LoanEntity> SelectLoanList(string status, string timeFrom, string timeTo, string pageSize, string pageIndex)
        {

            #region 数据验证

            var pIndex = ConvertHelper.ParseValue(pageIndex, 0);

            var pSize = ConvertHelper.ParseValue(pageSize, 0);

            var stts = ConvertHelper.ParseValue(status, 0);


            if (pSize <= 0) return null;
            if (pIndex <= 0) return null;

            var tf = ConvertHelper.ParseValue(timeFrom, DateTime.MinValue);
            var tt = ConvertHelper.ParseValue(timeTo, DateTime.MinValue);

            if ((tf == DateTime.MinValue) || (tt == DateTime.MinValue))
            {
                return null;
            }


            #endregion

            PageParam pp = new PageParam
            {
                PageCurrent = pIndex,
                PageSize = pSize
            };

            #region 验证缓存数据 tf.Ticks / 10000000 获取时间从最小时间到现在一共多少秒， 10000000=1秒

            var cacheKey = CacheHelper.GetCacheKeyByParam(pp, new IComparable[] { status, tf.Ticks / 10000000, tt.Ticks / 10000000 }).ToLower();
            var objEntity = CacheHelper.GetCache(cacheKey);

            if (objEntity == null)
            {
                var item = _dal.SelectLoanList(stts, timeFrom, timeTo, pp);
                CacheHelper.SetCache(cacheKey, item);
                return item;
            }

            #endregion

            return objEntity as Pagination<LoanEntity>;
        }
        public List<BorrowingEntity> SelectLoanList(string status)
        {
            var item = _dal.SelectLoanList(0);
            return item;


            //#region 数据验证


            //var stts = ConvertHelper.ParseValue(status, 0);



            //#endregion


            //#region 验证缓存数据 tf.Ticks / 10000000 获取时间从最小时间到现在一共多少秒， 10000000=1秒

            //var cacheKey = CacheHelper.GetCacheKeyByParam( new IComparable[] { status,"ttnz" }).ToLower();
            //var objEntity = CacheHelper.GetCache(cacheKey);

            //if (objEntity == null)
            //{
            //    var item = _dal.SelectLoanList(stts);
            //    CacheHelper.SetCache(cacheKey, item);
            //    return item;
            //}

            //#endregion

            //return objEntity as List<BorrowingEntity>;
        }


        /// <summary>
        /// 获取投资列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public Pagination<InvestmentEntity> SelectInvestmentList(string id, string pageSize, string pageIndex)
        {

            #region 数据验证

            var pIndex = ConvertHelper.ParseValue(pageIndex, 0);

            var pSize = ConvertHelper.ParseValue(pageSize, 0);
            var pId = ConvertHelper.ParseValue(id, 0);



            if (pSize <= 0 || pIndex <= 0 || pId <= 0) return null;



            #endregion

            PageParam pp = new PageParam
            {
                PageCurrent = pIndex,
                PageSize = pSize
            };




            #region 验证缓存数据

            var cacheKey = CacheHelper.GetCacheKeyByParam(pp, new IComparable[] { id }).ToLower();
            var objEntity = CacheHelper.GetCache(cacheKey);

            if (objEntity == null)
            {
                var item = _dal.SelectInvestmentList(pId, pp);
                CacheHelper.SetCache(cacheKey, item);
                return item;
            }

            #endregion

            return objEntity as Pagination<InvestmentEntity>;
        }
    }
}
