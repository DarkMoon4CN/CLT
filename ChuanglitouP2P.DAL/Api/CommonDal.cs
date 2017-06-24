using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;

using ChuangLiTou.Core.Entities.ProEnt;
using ChuangLiTou.Core.Entities.Response;
using ChuangLiTou.Core.Entities.Response.Capital;
using ChuangLiTou.Core.Entities.Response.Invest;
using ChuangLiTou.Core.Entities.Response.Member;
using ChuanglitouP2P.Common;
namespace ChuanglitouP2P.DAL.Api
{
    public static class CommonDal
    {
        /// <summary>
        /// 判断dataset是否为空
        /// </summary>
        /// <param name="ds"></param>
        /// <returns>true表示dataset不为空</returns>
        public static bool DataSetIsNotNull(DataSet ds)
        {
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取可用奖励
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static decimal GetBonuses(int userid)
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            decimal dec = 0.00M;
            //注需要过期时间处理         
            string sql = "select COALESCE(sum(amt),0) as amount_of_reward from hx_UserAct where registerid=" + userid.ToString() + " and UseState = 0 and AmtEndtime >='"+ today + "' and RewTypeID=2";
            var ds = DbHelper.Query(sql);
            if (DataSetIsNotNull(ds))
            {
                var dt = ds.Tables[0];
                dec = decimal.Parse(dt.Rows[0]["amount_of_reward"].ToString());
            }
            return dec;
        }
        /// <summary>
        /// 待收本金
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static decimal GetPrincipal(int userid)
        {
            decimal dec = 0.00M;
            string sql = "select  COALESCE(sum(investment_amount),0)  as investment_amount from hx_Bid_records where investor_registerid=" + userid.ToString() + " and payment_status=0 and ordstate=1 and IsLoans=1";
            DataTable dt = DbHelper.Query(sql).Tables[0];
            dec = decimal.Parse(dt.Rows[0]["investment_amount"].ToString());


            return dec;
        }
        /// <summary>
        /// 每个项目未付的利息(按日计息) 以当天为准计算.
        /// </summary>
        /// <param name="userid">The userid.</param>
        /// <returns>System.Decimal.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-17 13:27:51
        public static decimal GetUnpaidInterest(int userId)
        {
            decimal dec = 0M;
            string sql = "select  COALESCE(sum(withoutinterest),0) as withoutinterest from V_hx_Bid_records_borrowing_target  where investor_registerid=" + userId.ToString() + "  and tender_state between 2 and 5 and payment_status=0 and ordstate=1";
            var ds = DbHelper.Query(sql);
            dec = decimal.Parse(ds.Tables[0].Rows[0]["withoutinterest"].ToString());
            return dec;
        }
        /// <summary>
        /// 填充 资产总计 + 待收本金 + 待收收益 + 奖励金额
        /// <remarks>调用前必须已经调用了 <see cref="InitMemberEntity"/> 方法。</remarks>
        /// </summary>
        /// <param name="entity">需要填充的实体</param>
        /// <param name="userId">用户唯一标识</param>
        public static void FillMemberEntityAccountTotalAssets(ref MemberEntity entity, int userId)
        {
            entity.receivingIncome = GetUnpaidInterest(userId);//待收收益
            entity.totalAmount = GetPrincipal(userId);//待收本金
            entity.bonusAmount = GetBonuses(userId);//奖励金额

            //资产总计 = 账户余额 + 待收本金 + 待收收益 + 冻结金额 + 奖励金额
            entity.account_total_assets = entity.available_balance + entity.totalAmount + entity.receivingIncome + entity.frozen_sum + entity.bonusAmount;
        }
        /// <summary>
        /// 填充 资产总计 + 待收本金 + 待收收益 + 奖励金额
        /// <remarks>调用前必须已经调用了 <see cref="InitMemberEntity"/> 方法。</remarks>
        /// </summary>
        /// <param name="entity">需要填充的实体</param>
        /// <param name="userId">用户唯一标识</param>
        public static decimal GetAccountTotalAssets(decimal availableBalance, decimal frozen_sum, int userId)
        {
            var receivingIncome = GetUnpaidInterest(userId);//待收收益
            var totalAmount = GetPrincipal(userId);//待收本金
            var bonusAmount = GetBonuses(userId);//奖励金额

            //资产总计 = 账户余额 + 待收本金 + 待收收益 + 冻结金额 + 奖励金额
            return availableBalance + totalAmount + receivingIncome + frozen_sum + bonusAmount;
        }
    }
}
